using Microsoft.AspNet.Identity;
using PoolReservation.Database.Entity.Model;
using PoolReservation.Database.Entity.Model.Venue;
using PoolReservation.Database.Entity.Model.Venue.Incoming;
using PoolReservation.Database.Entity.Model.VenueType;
using PoolReservation.Database.Entity.Model.VenueType.IncomingVenueType;
using PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6;
using PoolReservation.Infrastructure.Errors;
using PoolReservation.Infrastructure.Http;
using PoolReservation.Models.Venue.Outgoing;
using PoolReservation.Models.Venues.Incoming;
using PoolReservation.Models.Venues.Outgoing;
using PoolReservation.Models.VenueType;
using PoolReservation.Models.VenueType.Outgoing;
using PoolReservation.SharedObjects.Model.Message.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace PoolReservation.api
{
    public class VenueController : ApiController
    {
        /// <summary>
        /// Gets a venue by id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Reservation/Venue")]
        [ResponseType(typeof(OutgoingVenue))]
        [Authorize]
        public HttpResponseMessage Venue(int id)
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

                    var venue = unitOfWork.Venues.GetVenueById(userId, id);

                    var outgoingVenues = OutgoingVenue.Parse(venue);

                    return JsonFactory.CreateJsonMessage(outgoingVenues, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }

        /// <summary>
        /// Get venues by hotel id.
        /// </summary>
        /// <param name="hotelId">The hotel identifier.</param>
        /// <param name="includeHidden">The include hidden.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Venue/ByHotelId")]
        [ResponseType(typeof(ICollection<OutgoingMinimalVenue>))]
        [Authorize]
        public HttpResponseMessage VenueByHotel(int hotelId, bool includeHidden = false)
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

                    var venues = unitOfWork.Venues.GetVenuesByHotelId(userId, hotelId, includeHidden).ToList();

                    var outgoingVenues = venues.Select(x => OutgoingMinimalVenue.Parse(x)).ToList();

                    return JsonFactory.CreateJsonMessage(outgoingVenues, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }


        #region add edit delete venue

        /// <summary>
        /// Adds the venue.
        /// </summary>
        /// <param name="venue">The venue.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("api/Venue/AddVenue")]
        [ResponseType(typeof(OutgoingVenue))]
        public HttpResponseMessage AddVenue(CreateVenues venue)
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
                    var returnedVenue = unitOfWork.Venues.CreateVenue(userId, venue);

                    unitOfWork.Complete();

                    try
                    {
                        returnedVenue = unitOfWork.Venues.GetVenueById(userId, returnedVenue.Id);
                    }
                    catch (Exception)
                    {

                    }

                    var outgoingVenue = OutgoingVenue.Parse(returnedVenue);

                    return JsonFactory.CreateJsonMessage(outgoingVenue, HttpStatusCode.OK, this.Request);
                }

            }, this.Request);
        }


        /// <summary>
        /// Edits the venue.
        /// </summary>
        /// <param name="addVenue">The add venue.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        [Route("api/Venue/EditVenue")]
        [ResponseType(typeof(OutgoingVenue))]
        public HttpResponseMessage EditVenue(IncomingEditVenue addVenue)
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
                    var editVenue = unitOfWork.Venues.EditVenue(userId, addVenue);

                    unitOfWork.Complete();

                    try
                    {
                        editVenue = unitOfWork.Venues.GetVenueById(userId, editVenue.Id);
                    }
                    catch (Exception)
                    {

                    }

                    var outgoingVenue = OutgoingVenue.Parse(editVenue);

                    return JsonFactory.CreateJsonMessage(outgoingVenue, HttpStatusCode.OK, this.Request);
                }

                
            }, this.Request);
        }


        // delete venue

        #endregion

        #region Blackouts

        /// <summary>
        /// Gets the blackouts for a venue.
        /// </summary>
        /// <param name="venueBlackout">The venue blackout parameters.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Venue/GetBlackoutForVenue")]
        [ResponseType(typeof(ICollection<OutgoingVenueBlackout>))]
        [Authorize]
        public HttpResponseMessage GetBlackoutsForVenue(IncomingBlackoutVenueCheck venueBlackout)
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

                    var blackouts = unitOfWork.Venues.GetBlackoutsForVenue(userId, venueBlackout.VenueId, venueBlackout.StartDate, venueBlackout.EndDate);

                    var outgoingBlackouts = blackouts.Select(x => OutgoingVenueBlackout.Parse(x)).ToList();

                    return JsonFactory.CreateJsonMessage(outgoingBlackouts, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }


        /// <summary>
        /// Deletes the blackout.
        /// </summary>
        /// <param name="blackoutId">The blackout identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        [Route("api/Venue/Blackout")]
        [ResponseType(typeof(OutgoingMessage))]
        public HttpResponseMessage DeleteBlackout(int blackoutId)
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

                    unitOfWork.Venues.DeleteBlackout(userId, blackoutId);

                    unitOfWork.Complete();

                    return JsonFactory.CreateJsonMessage(new OutgoingMessage { Message = "Success" }, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }


        /// <summary>
        /// Gets the blackout.
        /// </summary>
        /// <param name="blackoutId">The blackout identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("api/Venue/Blackout")]
        [ResponseType(typeof(OutgoingVenueBlackout))]
        public HttpResponseMessage GetBlackout(int blackoutId)
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

                    var blackout = unitOfWork.Venues.GetBlackout(userId, blackoutId);

                    var outgoingBlackout = OutgoingVenueBlackout.Parse(blackout);

                    return JsonFactory.CreateJsonMessage(outgoingBlackout, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }


        /// <summary>
        /// Edits the blackout.
        /// </summary>
        /// <param name="blackout">The blackout.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch]
        [Route("api/Venue/Blackout")]
        [ResponseType(typeof(OutgoingVenueBlackout))]
        public HttpResponseMessage EditBlackout(Blackout blackout)
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

                    var bl = unitOfWork.Venues.EditBlackout(userId, blackout);

                    unitOfWork.Complete();

                    var outgoingBlackout = OutgoingVenueBlackout.Parse(bl);

                    return JsonFactory.CreateJsonMessage(outgoingBlackout, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }

        /// <summary>
        /// Adds the blackout.
        /// </summary>
        /// <param name="blackout">The blackout.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("api/Venue/Blackout")]
        [ResponseType(typeof(OutgoingVenueBlackout))]
        public HttpResponseMessage AddBlackout(Blackout blackout)
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

                    var bl = unitOfWork.Venues.CreateBlackout(userId, blackout);

                    unitOfWork.Complete();

                    var outBl = new Blackout
                    {
                        Id = bl.Id,
                        StartDate = bl.StartDate,
                        EndDate = bl.EndDate,
                        VenueId = bl.VenueId
                    };

                    var outgoingBlackout = OutgoingVenueBlackout.Parse(outBl);

                    return JsonFactory.CreateJsonMessage(outgoingBlackout, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }

        #endregion

        #region venue type

        /// <summary>
        /// Get the Venue type by Hotel Id
        /// </summary>
        [HttpGet]
        [Route("api/Venue/GetVenueTypes")]
        [ResponseType(typeof(ICollection<OutgoingVenueType>))]
        [Authorize]
        public HttpResponseMessage GetVenueType(int hotelId)
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
                    var venueTypes = unitOfWork.VenueTypes.GetVenueTypesByHotelId(userId, hotelId);


                    var outgoingVenueType = venueTypes.Select(x => OutgoingVenueType.Parse(x)).ToList();

                    return JsonFactory.CreateJsonMessage(outgoingVenueType, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }


        /// <summary>
        /// Gets all venue item types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Venue/VenueTypes")]
        [ResponseType(typeof(ICollection<OutgoingVenueType>))]
        [Authorize]
        public HttpResponseMessage GetVenueTypes()
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
                    var venueTypes = unitOfWork.VenueTypes.GetVenueTypes(userId);

                    var outgoingVenueItemType = venueTypes.Select(x => OutgoingVenueType.Parse(x)).ToList();

                    return JsonFactory.CreateJsonMessage(outgoingVenueItemType, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }

        #endregion

        #region add edit delete venue type
        /// <summary>
        /// Adds the venue type.
        /// </summary>
        /// <param name="addVenueType">Type of the add venue.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("api/Venue/AddVenueType")]
        [ResponseType(typeof(OutgoingMessage))]
        public HttpResponseMessage AddVenueType(CreateVenueType addVenueType)
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
                    unitOfWork.VenueTypes.AddVenueType(userId, addVenueType);

                    unitOfWork.Complete();
                }

                return JsonFactory.CreateJsonMessage(new OutgoingMessage { Message = "Venue Type Added" }, HttpStatusCode.OK, this.Request);
            }, this.Request);
        }

        /// <summary>
        /// Edits the venue type.
        /// </summary>
        /// <param name="editVenueType">Type of the edit venue.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        [Route("api/Venue/EditVenueType")]
        [ResponseType(typeof(OutgoingMessage))]
        public HttpResponseMessage EditVenueType(IncomingEditVenueType editVenueType)
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
                    unitOfWork.VenueTypes.EditVenueType(userId, editVenueType);

                    unitOfWork.Complete();
                }

                return JsonFactory.CreateJsonMessage(new OutgoingMessage { Message = "Edited venue type" }, HttpStatusCode.OK, this.Request);
            }, this.Request);
        }

        //Delete the venue type

        #endregion
    }
}
