using Microsoft.AspNet.Identity;
using PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6;
using PoolReservation.Infrastructure.Errors;
using PoolReservation.Infrastructure.Http;
using PoolReservation.Infrastructure.Images;
using PoolReservation.Models.Icon.Outgoing;
using PoolReservation.SharedObjects.Model.File.Incoming;
using PoolReservation.SharedObjects.Model.Message.Outgoing;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PoolReservation.api
{
    public class AdminController : ApiController
    {
        /// <summary>
        /// Uploads an icon. 
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Admin/Icons")]
        [ResponseType(typeof(OutgoingIcon))]
        [Authorize]
        public async Task<HttpResponseMessage> AddIcon(IncomingBase64File model)
        {
            return await ErrorFactory.Handle(async () =>
            {
                var userID = User.Identity.GetUserId();

                if (userID == null)
                {
                    throw new Exception();
                }

                using (var unitOfWork = new UnitOfWork())
                {
                    try
                    {
                        if (model != null && !string.IsNullOrWhiteSpace(model.Data))
                        {
                            //This is added to help yogita with her base64 problems. 
                            model.Data = model.Data.Replace(' ', '+');


                            var data = ImageFactory.ConvertBase64ToArray(model.Data);

                            GalleryManager galMan = new GalleryManager();

                            var pictureId = await galMan.UploadImage(data, userID, ImageFormat.Png);

                            if (pictureId == null)
                            {
                                throw new Exception();
                            }

                            var icon = unitOfWork.Pictures.CreateIconLink(userID, pictureId ?? Guid.NewGuid());

                            unitOfWork.Complete();


                            return JsonFactory.CreateJsonMessage(OutgoingIcon.Parse(icon), HttpStatusCode.OK, this.Request);
                        }
                    }
                    catch (Exception)
                    {
                        //Maybe try to delete image.
                    }

                    return JsonFactory.CreateJsonMessage(new OutgoingMessage { Action = "unknownError" }, HttpStatusCode.InternalServerError, this.Request);
                }
            }, this.Request);
        }

        /// <summary>
        /// Deletes an icon.
        /// </summary>
        /// <param name="iconId">The link id.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/Admin/Icons")]
        [ResponseType(typeof(OutgoingMessage))]
        [Authorize]
        public HttpResponseMessage DeleteIcon(int iconId)
        {
            return ErrorFactory.Handle(() =>
            {
                var userID = User.Identity.GetUserId();

                if (userID == null)
                {
                    throw new Exception();
                }

                using (var unitOfWork = new UnitOfWork())
                {
                    unitOfWork.Pictures.DeleteIconLink(userID, iconId);

                    unitOfWork.Complete();

                    return JsonFactory.CreateJsonMessage(new OutgoingMessage { Action = "ok" }, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }

        /// <summary>
        /// Gets the icon.
        /// </summary>
        /// <param name="iconId">The icon identifier.</param>
        /// <returns></returns>

        [HttpGet]
        [Route("api/Admin/Icon")]
        [ResponseType(typeof(OutgoingIcon))]
        [Authorize]
        public HttpResponseMessage GetIcon(int iconId)
        {
            return ErrorFactory.Handle(() =>
            {
                var userID = User.Identity.GetUserId();

                if (userID == null)
                {
                    throw new Exception();
                }

                using (var unitOfWork = new UnitOfWork())
                {
                    var icon = unitOfWork.Pictures.GetIconLink(userID, iconId);

                    return JsonFactory.CreateJsonMessage(OutgoingIcon.Parse(icon), HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }

        [HttpGet]
        [Route("api/Admin/Icons")]
        [ResponseType(typeof(ICollection<OutgoingIcon>))]
        [Authorize]
        public HttpResponseMessage GetIcons()
        {
            return ErrorFactory.Handle(() =>
            {
                var userID = User.Identity.GetUserId();

                if (userID == null)
                {
                    throw new Exception();
                }

                using (var unitOfWork = new UnitOfWork())
                {
                    var icons = unitOfWork.Pictures.GetAllIcons(userID);

                    var outgoingIcons = icons?.Select(x => OutgoingIcon.Parse(x))?.ToList();

                    return JsonFactory.CreateJsonMessage(outgoingIcons, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }


    }
}
