using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Newtonsoft.Json.Linq;
using PoolReservation.App_Start;
using PoolReservation.Database.Entity;
using PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6;
using PoolReservation.Infrastructure.Errors;
using PoolReservation.Infrastructure.Http;
using PoolReservation.Infrastructure.Identity;
using PoolReservation.Infrastructure.Images;
using PoolReservation.Models.Authentication.Incoming;
using PoolReservation.Models.Permissions;
using PoolReservation.Models.User.Outgoing;
using PoolReservation.SharedObjects.Model.Authentication.Incoming;
using PoolReservation.SharedObjects.Model.Exceptions.Object;
using PoolReservation.SharedObjects.Model.Exceptions.Validation;
using PoolReservation.SharedObjects.Model.Message.Outgoing;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using static PoolReservation.Infrastructure.Identity.ExternalLoginData;

namespace PoolReservation.api
{
    public class AuthenticationController : ApiController
    {
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;

        public AuthenticationController()
        {
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? Request.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        [HttpGet]
        [Route("api/Authentication/Logout")]
        public HttpResponseMessage Logout()
        {

            return ErrorFactory.Handle(() =>
            {
                LogoutNoRoute();
                return JsonFactory.CreateJsonMessage(new OutgoingMessage { Message = "Logged out successfully!" }, HttpStatusCode.OK, this.Request);
            }, this.Request);
        }

        private void LogoutNoRoute()
        {
            Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
        }

        /// <summary>
        /// Allows the user to login, setting a cookie for the user. Returns the information of the user that just logged in.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Authentication/Login")]
        [ResponseType(typeof(OutgoingPersonalUser))]
        public async Task<HttpResponseMessage> Login(IncomingLogin credentials)
        {
            return await ErrorFactory.Handle(async () =>
            {
                var result = await SignInManager.PasswordSignInAsync(credentials.Email, credentials.Password, credentials.RememberMe, shouldLockout: true);

                switch (result)
                {
                    case SignInStatus.Success:
                        ApplicationUser user = await UserManager.FindByNameAsync(credentials.Email);
                        return GetInformationForCurrentUserResponseMessage(user.Id);
                    case SignInStatus.LockedOut:
                        return JsonFactory.CreateJsonMessage(new OutgoingMessage { Message = "Your account is locked out. Please try again later.", Action = "lockedOut" }, HttpStatusCode.Forbidden, Request);
                    case SignInStatus.RequiresVerification:
                        return JsonFactory.CreateJsonMessage(new OutgoingMessage { Message = "Your email requires verification. Please check your email and follow the instructions.", Action = "verificationRequired" }, HttpStatusCode.Forbidden, this.Request);
                    case SignInStatus.Failure:
                    default:
                        return JsonFactory.CreateJsonMessage(new OutgoingMessage { Message = "Invalid username/password combination", Action = "invalidCredentials" }, HttpStatusCode.Forbidden, this.Request);
                }
            }, this.Request);
        }



        [HttpGet]
        [Route("api/Authentication/ForgotPassword")]
        [ResponseType(typeof(OutgoingMessage))]
        public async Task<HttpResponseMessage> ForgotPassword(string email)
        {
            return await ErrorFactory.Handle(async () =>
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    throw new InvalidModelException();
                }


                var user = await UserManager.FindByEmailAsync(email);

                if (user == null)
                {
                    return JsonFactory.CreateJsonMessage(new OutgoingMessage { Message = "ok" }, HttpStatusCode.OK, this.Request);
                }

                var token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

                await this.SendForgotPasswordEmail(user, token);

                return JsonFactory.CreateJsonMessage(new OutgoingMessage { Message = "ok" }, HttpStatusCode.OK, this.Request);
            }, this.Request);
        }

        [HttpPost]
        [Route("api/Authentication/ForgotPasswordWithToken")]
        [ResponseType(typeof(OutgoingMessage))]
        public async Task<HttpResponseMessage> ForgotPasswordWithToken(IncomingForgotPasswordWithToken fpDetails)
        {
            return await ErrorFactory.Handle(async () =>
            {
                if (fpDetails == null || string.IsNullOrWhiteSpace(fpDetails.Email) || string.IsNullOrWhiteSpace(fpDetails.Token) || string.IsNullOrWhiteSpace(fpDetails.Password))
                {
                    throw new InvalidModelException() { Action = "nullData" };
                }

                var pwResult = await UserManager.PasswordValidator.ValidateAsync(fpDetails.Password);

                if (pwResult.Succeeded == false)
                {
                    throw new InvalidModelException { Action = "invalidPassword" };
                }

                var user = await UserManager.FindByEmailAsync(fpDetails.Email);

                if (user == null)
                {
                    throw new Exception();
                }

                var result = await UserManager.ResetPasswordAsync(user.Id, fpDetails.Token, fpDetails.Password);

                if (result.Succeeded == false)
                {
                    throw new Exception();
                }

                return JsonFactory.CreateJsonMessage(new OutgoingMessage { Message = "Success!" }, HttpStatusCode.OK, this.Request);
            }, this.Request);
        }

        private async Task SendForgotPasswordEmail(ApplicationUser user, string token)
        {
            var url = $"https://{HttpContext.Current.Request.Url.Authority}/#/auth/forgotpassword?token={HttpUtility.UrlEncode(token)}";

            string apiKey = ConfigurationManager.AppSettings["SendgridKey"];
            dynamic sg = new SendGridAPIClient(apiKey);

            Email from = new Email(ConfigurationManager.AppSettings["SendgridEmail"]);
            string subject = "Forgot Password";
            Email to = new Email(user.Email);
            Content content = new Content("text/html", $"Hello {user.FirstName} {user.LastName}, <br><br>To reset your password, please <a href=\"{url}\">click here</a>. <br><br> If this link does not work, copy and paste this url into your browser: <br><br> {url}");
            Mail mail = new Mail(from, subject, to, content);

            dynamic response = await sg.client.mail.send.post(requestBody: mail.Get());
        }

        private HttpResponseMessage GetInformationForCurrentUserResponseMessage()
        {
            var userID = User.Identity.GetUserId();

            return GetInformationForCurrentUserResponseMessage(userID);
        }

        private HttpResponseMessage GetInformationForCurrentUserResponseMessage(string userID)
        {

            if (string.IsNullOrWhiteSpace(userID))
            {
                try
                {
                    LogoutNoRoute();
                }
                catch (Exception)
                {

                }

                return JsonFactory.CreateJsonMessage(new OutgoingMessage { Message = "An unknown error has occured.", Action = "unknownError" }, HttpStatusCode.Forbidden, this.Request);
            }

            using (var unitOfWork = new UnitOfWork())
            {
                var realUserInformation = unitOfWork.Users.GetUserByIdForLogin(userID, userID);

                if (realUserInformation == null)
                {
                    return JsonFactory.CreateJsonMessage(new OutgoingMessage { Message = "An unknown error has occured.", Action = "unknownError" }, HttpStatusCode.Forbidden, this.Request);
                }

                var outgoingInformation = OutgoingPersonalUser.Parse(realUserInformation);

                return JsonFactory.CreateJsonMessage(outgoingInformation, HttpStatusCode.OK, this.Request);
            }
        }

        /// <summary>
        /// Registers the userId.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Authentication/Register")]
        [ResponseType(typeof(OutgoingPersonalUser))]
        public async Task<HttpResponseMessage> Register(IncomingRegister model)
        {
            return await ErrorFactory.Handle(async () =>
            {
                var pwResult = await UserManager.PasswordValidator.ValidateAsync(model.Password);

                if (pwResult.Succeeded == false)
                {
                    return JsonFactory.CreateJsonMessage(new OutgoingMessage { Message = "Registration failed", Action = "invalidPassword" }, HttpStatusCode.Forbidden, this.Request);
                }

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber, FirstName = model.FirstName, LastName = model.LastName, DateCreated = DateTime.UtcNow, SitePermissionsId = Convert.ToInt32(SitePermissionsEnum.REGULAR_APP_USER) };
                var result = await UserManager.CreateAsync(user, model.Password);



                if (result.Succeeded)
                {
                    try
                    {
                        if (model.Image != null && !string.IsNullOrWhiteSpace(model.Image.Data))
                        {
                            //This is added to help yogita with her base64 problems. 
                            model.Image.Data = model.Image.Data.Replace(' ', '+');


                            var data = ImageFactory.ConvertBase64ToArray(model.Image.Data);

                            GalleryManager galMan = new GalleryManager();

                            var pictureId = await galMan.UploadImage(data, user.Id);

                            if (pictureId == null)
                            {
                                throw new Exception();
                            }

                            using (var unitOfWork = new UnitOfWork())
                            {
                                unitOfWork.Users.SetProfilePictureADMIN(user.Id, pictureId ?? Guid.NewGuid());

                                unitOfWork.Complete();
                            }
                        }
                    }
                    catch (Exception)
                    {
                        //Maybe try to delete image.
                    }

                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);


                    return GetInformationForCurrentUserResponseMessage(user.Id);
                }

                return JsonFactory.CreateJsonMessage(new OutgoingMessage { Message = "Registration failed", Action = "unknownFailure" }, HttpStatusCode.Forbidden, this.Request);

            }, this.Request);
        }

        /// <summary>
        /// Registers the userId.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Authentication/ChangePassword")]
        [ResponseType(typeof(OutgoingPersonalUser))]
        [Authorize]
        public async Task<HttpResponseMessage> ChangePassword(IncomingChangePassword model)
        {
            return await ErrorFactory.Handle(async () =>
            {

                var userID = User.Identity.GetUserId();

                if (userID == null)
                {
                    throw new Exception();
                }

                var pwResult = await UserManager.PasswordValidator.ValidateAsync(model.NewPassword);

                if (pwResult.Succeeded == false)
                {
                    return JsonFactory.CreateJsonMessage(new OutgoingMessage { Message = "Invalid password", Action = "invalidPassword" }, HttpStatusCode.Forbidden, this.Request);
                }

                var result = await UserManager.ChangePasswordAsync(userID, model.CurrentPassword, model.NewPassword);

                if(result.Succeeded == false)
                {
                    return JsonFactory.CreateJsonMessage(new OutgoingMessage { Message = "Invalid username password combo", Action = "invalidUsernamePassword" }, HttpStatusCode.Forbidden, this.Request);
                }

                return GetInformationForCurrentUserResponseMessage(userID);

            }, this.Request);
        }

        /// <summary>
        /// Registers the userId.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Authentication/User/Edit")]
        [ResponseType(typeof(OutgoingPersonalUser))]
        [Authorize]
        public async Task<HttpResponseMessage> EditUser(IncomingEditUser model)
        {
            return await ErrorFactory.Handle(async () =>
            {
                var userID = User.Identity.GetUserId();

                if(userID == null)
                {
                    throw new Exception();
                }

                using (var unitOfWork = new UnitOfWork())
                {

                    unitOfWork.Users.EditUser(userID, model.PhoneNumber, model.FirstName, model.LastName);

                    unitOfWork.Complete();

                    try
                    {
                        if (model.Image != null && !string.IsNullOrWhiteSpace(model.Image.Data))
                        {
                            //This is added to help yogita with her base64 problems. 
                            model.Image.Data = model.Image.Data.Replace(' ', '+');


                            var data = ImageFactory.ConvertBase64ToArray(model.Image.Data);

                            GalleryManager galMan = new GalleryManager();

                            var pictureId = await galMan.UploadImage(data, userID);

                            if (pictureId == null)
                            {
                                throw new Exception();
                            }

                            unitOfWork.Users.SetProfilePictureADMIN(userID, pictureId ?? Guid.NewGuid());

                            unitOfWork.Complete();

                        }
                    }
                    catch (Exception)
                    {
                        //Maybe try to delete image.
                    }


                    return GetInformationForCurrentUserResponseMessage(userID);

                }
            }, this.Request);
        }

        /// <summary>
        /// Extends the cookie lifetime of the user, and get's the users updated information.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Authentication/Revalidate")]
        [Authorize]
        [ResponseType(typeof(OutgoingPersonalUser))]
        public HttpResponseMessage Revalidate()
        {
            return ErrorFactory.Handle(() =>
            {
                return GetInformationForCurrentUserResponseMessage();

            }, this.Request);
        }

        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("api/Authentication/ForJonah/RedirectToExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            string redirectUri = "http://localhost:54046/";

            if (error != null)
            {
                return BadRequest(Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            //Fix this.

            //var redirectUriValidationResult = ValidateClientAndRedirectUri(this.Request, ref redirectUri);

            //if (!string.IsNullOrWhiteSpace(redirectUriValidationResult))
            //{
            //    return BadRequest(redirectUriValidationResult);
            //}

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }


            IdentityUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider, externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            redirectUri = string.Format("{0}#external_access_token={1}&provider={2}&haslocalaccount={3}&external_user_name={4}",
                                            redirectUri,
                                            externalLogin.ExternalAccessToken,
                                            externalLogin.LoginProvider,
                                            hasRegistered.ToString(),
                                            externalLogin.UserName);

            return Redirect(redirectUri);

        }

        private async Task<ParsedExternalAccessToken> VerifyExternalAccessToken(string provider, string accessToken)
        {
            ParsedExternalAccessToken parsedToken = null;

            var verifyTokenEndPoint = "";

            if (provider == "Facebook")
            {
                //You can get it from here: https://developers.facebook.com/tools/accesstoken/
                //More about debug_tokn here: http://stackoverflow.com/questions/16641083/how-does-one-get-the-app-access-token-for-debug-token-inspection-on-facebook

                var appToken = ConfigurationManager.AppSettings["FacebookDebugToken"];
                verifyTokenEndPoint = $"https://graph.facebook.com/debug_token?input_token={accessToken}&access_token={appToken}";
            }
            //else if (provider == "Google")
            //{
            //    verifyTokenEndPoint = string.Format("https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={0}", accessToken);
            //}
            else
            {
                return null;
            }

            var client = new HttpClient();
            var uri = new Uri(verifyTokenEndPoint);
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                dynamic jObj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                parsedToken = new ParsedExternalAccessToken();

                if (provider == "Facebook")
                {
                    parsedToken.user_id = jObj["data"]["user_id"];
                    parsedToken.app_id = jObj["data"]["app_id"];

                    if (!string.Equals(Startup.facebookAuthOptions.AppId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }
                }
                else if (provider == "Google")
                {
                    //parsedToken.user_id = jObj["user_id"];
                    //parsedToken.app_id = jObj["audience"];

                    //if (!string.Equals(Startup.googleAuthOptions.ClientId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                    //{
                    //    return null;
                    //}

                }

            }

            return parsedToken;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Authentication/ObtainLocalAccessToken")]
        [ResponseType(typeof(OutgoingPersonalUser))]
        public async Task<HttpResponseMessage> ObtainLocalAccessToken(string provider, string externalAccessToken)
        {
            return await ErrorFactory.Handle(async () =>
           {
               if (string.IsNullOrWhiteSpace(provider) || string.IsNullOrWhiteSpace(externalAccessToken))
               {
                   throw new InvalidModelException() { Action = "nullData" };
               }

               var verifiedAccessToken = await VerifyExternalAccessToken(provider, externalAccessToken);
               if (verifiedAccessToken == null)
               {
                   throw new InvalidModelException() { Action = "failedVerification" };
               }

               ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(provider, verifiedAccessToken.user_id));

               bool hasRegistered = user != null;

               if (!hasRegistered)
               {
                   throw new InvalidModelException() { Action = "notRegistered" };
               }

               await SignInManager.SignInAsync(user, true, true);

               return GetInformationForCurrentUserResponseMessage(user.Id);

           }, this.Request);


        }

        [Authorize]
        [HttpDelete]
        [Route("api/Authentication/DeleteExternalLinksForProvider")]
        [ResponseType(typeof(OutgoingMessage))]
        public  HttpResponseMessage DeleteExternalLinksForProvider(string provider)
        {
            return ErrorFactory.Handle(() =>
            {
                var userId = User?.Identity?.GetUserId();

                if (userId == null)
                {
                    throw new Exception("User not found.");
                }

                using (var unitOfWork = new UnitOfWork())
                {
                    unitOfWork.Users.DeleteSocialLinkingsForProviderSYSTEM(userId, provider);
                    unitOfWork.Complete();
                }

                return JsonFactory.CreateJsonMessage(new OutgoingMessage { Message = $"Social linkings for {provider} deleted."}, HttpStatusCode.OK, this.Request);

            }, this.Request);


        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/Authentication/RegisterExternal")]
        [ResponseType(typeof(OutgoingPersonalUser))]
        public async Task<HttpResponseMessage> RegisterExternal(IncomingExternalRegister model)
        {
            return await ErrorFactory.Handle(async () =>
            {

                if (model == null || string.IsNullOrWhiteSpace(model.Provider) || string.IsNullOrWhiteSpace(model.ExternalToken))
                {
                    throw new InvalidModelException() { Action = "nullData" };
                }

                var pwResult = await UserManager.PasswordValidator.ValidateAsync(model.Password);

                if (pwResult.Succeeded == false)
                {
                    throw new InvalidModelException() { Action = "invalidPassword" };
                }

                var verifiedAccessToken = await VerifyExternalAccessToken(model.Provider, model.ExternalToken);
                if (verifiedAccessToken == null)
                {
                    throw new InvalidModelException() { Action = "failedVerification" };
                }

                ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(model.Provider, verifiedAccessToken.user_id));

                bool hasRegistered = user != null;

                if (hasRegistered)
                {
                    throw new InvalidModelException() { Action = "alreadyRegistered" };
                }

                var emailAlreadyExists = await UserManager.FindByEmailAsync(model.Email) != null;

                if (emailAlreadyExists)
                {
                    throw new InvalidModelException() { Action = "emailAlreadyRegistered" };
                }

                user = new ApplicationUser() { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, PhoneNumber = model.PhoneNumber, DateCreated = DateTime.UtcNow, SitePermissionsId = Convert.ToInt32(SitePermissionsEnum.REGULAR_APP_USER) };

                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    throw new Exception();
                }

                var info = new ExternalLoginInfo()
                {
                    DefaultUserName = model.Email,
                    Login = new UserLoginInfo(model.Provider, verifiedAccessToken.user_id)
                };

                result = await UserManager.AddLoginAsync(user.Id, info.Login);
                if (!result.Succeeded)
                {

                    try
                    {
                        await UserManager.DeleteAsync(user);
                    }
                    catch (Exception)
                    {

                    }

                    throw new Exception();
                }



                await SignInManager.SignInAsync(user, true, true);

                try
                {
                    if (model.Image != null && !string.IsNullOrWhiteSpace(model.Image.Data))
                    {
                        //This is added to help yogita with her base64 problems. 
                        model.Image.Data = model.Image.Data.Replace(' ', '+');


                        var data = ImageFactory.ConvertBase64ToArray(model.Image.Data);

                        GalleryManager galMan = new GalleryManager();

                        var pictureId = await galMan.UploadImage(data, user.Id);

                        if (pictureId == null)
                        {
                            throw new Exception();
                        }

                        using (var unitOfWork = new UnitOfWork())
                        {
                            unitOfWork.Users.SetProfilePictureADMIN(user.Id, pictureId ?? Guid.NewGuid());

                            unitOfWork.Complete();
                        }
                    }
                }
                catch (Exception)
                {
                    //Maybe try to delete image.
                }

                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);


                return GetInformationForCurrentUserResponseMessage(user.Id);


            }, this.Request);


        }
    }


}