using Microsoft.AspNet.Identity;
using PoolReservation.Database.Entity.Model;
using PoolReservation.Database.Entity.Model.Hotel;
using PoolReservation.Database.Entity.Model.Hotel.Incoming;
using PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6;
using PoolReservation.Helpers;
using PoolReservation.Infrastructure.Errors;
using PoolReservation.Infrastructure.Http;
using PoolReservation.Infrastructure.Images;
using PoolReservation.Models.Hotel.Incoming;
using PoolReservation.Models.Hotel.Outgoing;
using PoolReservation.Models.Location.Incoming;
using PoolReservation.Models.Permissions.Incoming;
using PoolReservation.Models.Permissions.Outgoing;
using PoolReservation.Models.User.Incoming;
using PoolReservation.Models.User.Outgoing;
using PoolReservation.SharedObjects.Model.Exceptions.Validation;
using PoolReservation.SharedObjects.Model.Message.Outgoing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PoolReservation.api
{
    public class HotelController : ApiController
    {

        /// <summary>
        /// Searches the hotels.
        /// </summary>
        /// <param name="hotelQuery">The hotel query.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("api/Hotel/Search")]
        [ResponseType(typeof(ICollection<OutgoingDetailedHotel>))]
        public HttpResponseMessage SearchHotels(IncomingSearchHotels hotelQuery)
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
                    var hotels = unitOfWork.Hotels.SearchHotels(userId, hotelQuery.query, hotelQuery.startingIndex, hotelQuery.numberToGet, hotelQuery.IncludeHidden);

                    var outgoingHotels = hotels.ToList().Select(x => OutgoingDetailedHotel.Parse(x)).ToList();

                    return JsonFactory.CreateJsonMessage(outgoingHotels, HttpStatusCode.OK, this.Request);
                }




            }, this.Request);

        }

        /// <summary>
        /// Searches the users in the hotel.
        /// </summary>
        /// <param name="userQuery">The parameters.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("api/Hotel/Users/Search")]
        [ResponseType(typeof(ICollection<OutgoingHotelUser>))]
        public HttpResponseMessage SearchEmployees(IncomingHotelUserSearch userQuery)
        {
            return ErrorFactory.Handle(() =>
            {
                var userId = User?.Identity?.GetUserId();

                if (userId == null)
                {
                    throw new Exception("User not found.");
                }

                if (userQuery == null)
                {
                    throw new InvalidModelException("hotelQuery cannot be null");
                }

                using (var unitOfWork = new UnitOfWork())
                {
                    var hotels = unitOfWork.Hotels.SearchHotelUsers(userId, userQuery.HotelId, userQuery.Query, userQuery.StartsWith, userQuery.NumberToGet);

                    var outgoingHotels = hotels.ToList().Select(x => OutgoingHotelUser.Parse(x)).ToList();

                    return JsonFactory.CreateJsonMessage(outgoingHotels, HttpStatusCode.OK, this.Request);
                }




            }, this.Request);

        }

        /// <summary>
        /// Gets the users in a hotel
        /// </summary>
        /// <param name="userRequest">The parameters.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("api/Hotel/Users/")]
        [ResponseType(typeof(ICollection<OutgoingHotelUser>))]
        public HttpResponseMessage GetEmployees(IncomingHotelUserRequest userRequest)
        {
            return ErrorFactory.Handle(() =>
            {
                var userId = User?.Identity?.GetUserId();

                if (userId == null)
                {
                    throw new Exception("User not found.");
                }

                if (userRequest == null)
                {
                    throw new InvalidModelException("hotelQuery cannot be null");
                }

                using (var unitOfWork = new UnitOfWork())
                {
                    var hotels = unitOfWork.Hotels.GetHotelUsers(userId, userRequest.HotelId, userRequest.StartsWith, userRequest.NumberToGet);

                    var outgoingHotels = hotels.ToList().Select(x => OutgoingHotelUser.Parse(x)).ToList();

                    return JsonFactory.CreateJsonMessage(outgoingHotels, HttpStatusCode.OK, this.Request);
                }




            }, this.Request);

        }

        /// <summary>
        /// Gets the users information inside of the hotel.
        /// </summary>
        /// <param name="hotelId">The hotel identifier.</param>
        /// <param name="userIdToGet">The user identifier to get.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Hotel/{hotelId}/ByUserId/{userIdToGet}")]
        [ResponseType(typeof(OutgoingHotelUser))]
        [Authorize]
        public HttpResponseMessage GetUserInHotel(int hotelId, string userIdToGet)
        {
            return ErrorFactory.Handle(() =>
            {
                var userId = User?.Identity?.GetUserId();

                if (string.IsNullOrWhiteSpace(userId))
                {
                    throw new Exception();
                }

                using (var unitOfWork = new UnitOfWork())
                {
                    var user = unitOfWork.Hotels.GetHotelUser(userId, hotelId, userIdToGet);

                    var outgoingUser = OutgoingHotelUser.Parse(user);

                    return JsonFactory.CreateJsonMessage(outgoingUser, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }

        /// <summary>
        /// Gets all of the hotels that the current user is associated with
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Hotel/ByCurrentUser")]
        [ResponseType(typeof(ICollection<OutgoingHotel>))]
        [Authorize]
        public HttpResponseMessage HotelsByCurrentUser()
        {
            return ErrorFactory.Handle(() =>
            {
                var userId = User?.Identity?.GetUserId();

                if (string.IsNullOrWhiteSpace(userId))
                {
                    throw new Exception();
                }

                using (var unitOfWork = new UnitOfWork())
                {
                    var hotels = unitOfWork.Hotels.GetHotelsByUserId(userId).ToList();

                    var outgoingHotels = hotels.Select(x => OutgoingHotel.Parse(x)).ToList();

                    return JsonFactory.CreateJsonMessage(outgoingHotels, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }



        #region Reservations
        [HttpGet]
        [Route("api/Hotel")]
        [ResponseType(typeof(OutgoingHotel))]
        [Authorize]
        public HttpResponseMessage Hotel(int id)
        {
            return ErrorFactory.Handle(() =>
            {
                var currentUserId = User?.Identity?.GetUserId();

                if (string.IsNullOrWhiteSpace(currentUserId))
                {
                    throw new Exception();
                }

                using (var unitOfWork = new UnitOfWork())
                {
                    var hotel = unitOfWork.Hotels.GetHotelById(currentUserId, id);

                    var outgoingHotel = OutgoingHotel.Parse(hotel);

                    return JsonFactory.CreateJsonMessage(outgoingHotel, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }

        /// <summary>
        /// Gets all hotels that are within the specified distance from the specified location.
        /// </summary>
        /// <param name="location">The location of the user/city and the distance to search.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Reservation/Hotel/ByLocation/InRadius")]
        [ResponseType(typeof(ICollection<OutgoingHotel>))]
        [Authorize]
        public HttpResponseMessage Hotel(IncomingBoundingBoxLocation location)
        {

            return ErrorFactory.Handle(() =>
            {
                var currentUserId = User?.Identity?.GetUserId();

                if (string.IsNullOrWhiteSpace(currentUserId))
                {
                    throw new UnauthorizedAccessException();
                }

                using (var unitOfWork = new UnitOfWork())
                {
                    var theLocation = LocationBounded.FromDegrees(location.Latitude, location.Longitude);

                    var boundingBox = theLocation.BoundingCoordinates(location.Distance);

                    if (boundingBox.Length != 2)
                    {
                        throw new Exception("The bounding box is not in the correct format.");
                    }

                    var bottomLeft = boundingBox[0].ConvertToLatLonLocation();
                    var topRight = boundingBox[1].ConvertToLatLonLocation();

                    var hotels = unitOfWork.Hotels.GetHotelsInRadius(currentUserId, bottomLeft, topRight, location.Distance).ToList();

                    var outgoingHotels = hotels.Select(x => OutgoingHotel.Parse(x)).ToList();

                    return JsonFactory.CreateJsonMessage(outgoingHotels, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }

        /// <summary>
        /// Get's all of the hotels that a specified user is associated with.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Hotel/ByUserId")]
        [ResponseType(typeof(ICollection<OutgoingHotel>))]
        [Authorize]
        public HttpResponseMessage HotelsByUserId(string userId)
        {
            return ErrorFactory.Handle(() =>
            {
                var currentUserId = User?.Identity?.GetUserId();

                if (string.IsNullOrWhiteSpace(currentUserId))
                {
                    throw new Exception();
                }

                using (var unitOfWork = new UnitOfWork())
                {
                    var hotels = unitOfWork.Hotels.GetHotelsByUserId(userId).ToList();

                    var outgoingHotels = hotels.Select(x => OutgoingHotel.Parse(x)).ToList();

                    return JsonFactory.CreateJsonMessage(outgoingHotels, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }
        #endregion


        #region add edit delete

        /// <summary>
        /// Adds the hotels.
        /// </summary>
        /// <param name="hotel">The hotel.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("api/Hotel/AddHotel")]
        [ResponseType(typeof(OutgoingHotel))]
        public async Task<HttpResponseMessage> AddHotels(CreateHotels hotel)
        {
            return await ErrorFactory.Handle(async () =>
            {
                var userId = User?.Identity?.GetUserId();

                if (userId == null)
                {
                    throw new Exception("User not found.");
                }

                if (hotel == null)
                {
                    throw new InvalidModelException("hotel cannot be null");
                }

                if (hotel.TaxRate < 0 || hotel.TaxRate > 1)
                {
                    throw new InvalidModelException("Tax rate must be greater than 0 and less than 1");
                }

                using (var unitOfWork = new UnitOfWork())
                {
                    var finHotel = unitOfWork.Hotels.CreateHotel(userId, hotel);

                    unitOfWork.Complete();

                    try
                    {
                        if (hotel.Image != null && !string.IsNullOrWhiteSpace(hotel.Image.Data))
                        {
                            //This is added to help yogita with her base64 problems. 
                            hotel.Image.Data = hotel.Image.Data.Replace(' ', '+');


                            var data = ImageFactory.ConvertBase64ToArray(hotel.Image.Data);

                            GalleryManager galMan = new GalleryManager();

                            var pictureId = await galMan.UploadImage(data, userId);

                            if (pictureId == null)
                            {
                                throw new Exception();
                            }

                            var tempHotel = unitOfWork.Hotels.SetHotelImageADMIN(userId, finHotel.Id, pictureId ?? Guid.NewGuid());

                            unitOfWork.Complete();
                            finHotel = tempHotel;
                        }
                    }
                    catch (Exception)
                    {
                        //Maybe try to delete image.
                    }


                    return JsonFactory.CreateJsonMessage(OutgoingHotel.Parse(finHotel), HttpStatusCode.OK, this.Request);
                }

            }, this.Request);

        }

        /// <summary>
        /// Edits  the hotels.
        /// </summary>
        /// <param name = "hotel" > The hotel.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("api/Hotel/EditHotel")]
        [ResponseType(typeof(OutgoingHotel))]
        public async Task<HttpResponseMessage> EditHotel(IncomingEditHotel hotel)
        {
            return await ErrorFactory.Handle(async () =>
            {
                var userId = User?.Identity?.GetUserId();

                if (userId == null)
                {
                    throw new Exception("User not found.");
                }

                if (hotel == null)
                {
                    throw new InvalidModelException("hotel cannot be null");
                }

                if (hotel.TaxRate < 0 || hotel.TaxRate > 1)
                {
                    throw new InvalidModelException("Tax rate must be greater than 0 and less than 1");
                }

                using (var unitOfWork = new UnitOfWork())
                {
                    var finHotel = unitOfWork.Hotels.EditHotel(userId, hotel);

                    unitOfWork.Complete();

                    try
                    {
                        if (hotel.Image != null && !string.IsNullOrWhiteSpace(hotel.Image.Data))
                        {
                            //This is added to help yogita with her base64 problems. 
                            hotel.Image.Data = hotel.Image.Data.Replace(' ', '+');


                            var data = ImageFactory.ConvertBase64ToArray(hotel.Image.Data);

                            GalleryManager galMan = new GalleryManager();

                            var pictureId = await galMan.UploadImage(data, userId);

                            if (pictureId == null)
                            {
                                throw new Exception();
                            }

                            var tempHotel = unitOfWork.Hotels.SetHotelImageADMIN(userId, finHotel.Id, pictureId ?? Guid.NewGuid());

                            unitOfWork.Complete();
                            finHotel = tempHotel;
                        }
                    }
                    catch (Exception)
                    {
                        //Maybe try to delete image.
                    }

                    return JsonFactory.CreateJsonMessage(OutgoingHotel.Parse(finHotel), HttpStatusCode.OK, this.Request);

                }
            }, this.Request);

        }

        //delete hotel route

        #endregion


        #region Hotel Permissions
        [Authorize]
        [HttpGet]
        [Route("api/Hotel/Permissions/")]
        [ResponseType(typeof(OutgoingHotelPermissions))]
        public HttpResponseMessage GetHotelPermissionsObject()
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
                    var perms = unitOfWork.Hotels.GetHotelPermissions();

                    var outgoingperms = perms.ToList().Select(x => OutgoingHotelPermissions.Parse(x)).ToList();

                    return JsonFactory.CreateJsonMessage(outgoingperms, HttpStatusCode.OK, this.Request);
                }




            }, this.Request);

        }

        /// <summary>
        /// Gets the hotel permissions for the current user.
        /// </summary>
        /// <param name="hotelId">The hotel identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("api/Hotel/User/Permissions/{hotelId}")]
        [ResponseType(typeof(OutgoingHotelPermissions))]
        public HttpResponseMessage GetHotelPermissionsObjectForUser(int hotelId)
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
                    var perms = unitOfWork.Hotels.GetPermissionsForUserAtHotel(userId, hotelId);

                    var outgoingperms = OutgoingHotelPermissions.Parse(perms);

                    return JsonFactory.CreateJsonMessage(outgoingperms, HttpStatusCode.OK, this.Request);
                }




            }, this.Request);

        }

        [Authorize]
        [HttpPatch]
        [Route("api/Hotel/User/Permissions/")]
        [ResponseType(typeof(OutgoingHotelUser))]
        public HttpResponseMessage ChangePermissionsForUser(IncomingHotelPermissionsChange userToChange)
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
                    var user = unitOfWork.Hotels.ChangeUserPermissionsInHotel(userId, userToChange.HotelId, userToChange.UserIdToChange, userToChange.PermissionsId);

                    unitOfWork.Complete();

                    var outgoinguser = OutgoingHotelUser.Parse(user);

                    return JsonFactory.CreateJsonMessage(outgoinguser, HttpStatusCode.OK, this.Request);
                }




            }, this.Request);

        }

        [Authorize]
        [HttpPost]
        [Route("api/Hotel/User/Permissions/")]
        [ResponseType(typeof(OutgoingHotelUser))]
        public HttpResponseMessage AddPermissionsForUser(IncomingHotelPermissionsChange userToChange)
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
                    var user = unitOfWork.Hotels.AddUserPermissionsInHotel(userId, userToChange.UserIdToChange, userToChange.HotelId, userToChange.PermissionsId);

                    unitOfWork.Complete();

                    var outgoinguser = OutgoingHotelUser.Parse(user);

                    return JsonFactory.CreateJsonMessage(outgoinguser, HttpStatusCode.OK, this.Request);
                }

            }, this.Request);

        }
        #endregion
    }
}
