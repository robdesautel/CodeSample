using PoolReservation.Database.Entity.Model.Item;
using PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6;
using PoolReservation.Infrastructure.Errors;
using PoolReservation.Infrastructure.Http;
using PoolReservation.Models.VenueItem.Outgoing;
using PoolReservation.Models.VItems.Incoming;
using PoolReservation.Models.VenueItemType.Incoming;
using PoolReservation.Models.VenueItemType.Outgoing;
using PoolReservation.SharedObjects.Model.Message.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PoolReservation.Database.Entity.Model.Item.Incoming;
using Microsoft.AspNet.Identity;
using PoolReservation.Database.Entity.Model.ItemType;
using PoolReservation.Database.Entity.Model.ItemType.Incoming;

namespace PoolReservation.api
{
    public class VenueItemsController : ApiController
    {

        [HttpGet]
        [Route("api/VenueItems")]
        [ResponseType(typeof(OutgoingVenueItems))]
        [Authorize]
        public HttpResponseMessage VenueItem(int id)
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
                    var venueItems = unitOfWork.VenueItems.GetVenueItemById(userId, id);

                    var outgoingVenueItems = OutgoingVenueItems.Parse(venueItems);

                    return JsonFactory.CreateJsonMessage(outgoingVenueItems, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }

        [HttpGet]
        [Route("api/VenueItems/WithQuantity")]
        [ResponseType(typeof(OutgoingVenueItems))]
        [Authorize]
        public HttpResponseMessage VenueItemWithQuantity(int id)
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
                    var venueItems = unitOfWork.VenueItems.GetVenueItemWithQuantityById(userId, id);

                    var outgoingVenueItems = OutgoingVenueItems.Parse(venueItems);

                    return JsonFactory.CreateJsonMessage(outgoingVenueItems, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }


        /// <summary>
        /// Get venueItems by VenueId.
        /// </summary>
        /// <param name="venueId">The venue identifier.</param>
        /// <param name="includeHidden">The include hidden.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/VenueItems/ByVenueId")]
        [ResponseType(typeof(ICollection<OutgoingVenueItems>))]
        [Authorize]
        public HttpResponseMessage VenueItemByVenue(int venueId, bool includeHidden = false)
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
                    var venueItems = unitOfWork.VenueItems.GetVenueItemsByVenueId(userId, venueId, includeHidden).ToList();

                    var outgoingVenueItems = venueItems.Select(x => OutgoingVenueItems.Parse(x)).ToList();

                    return JsonFactory.CreateJsonMessage(outgoingVenueItems, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }

        /// <summary>
        /// Get Venue Types by VenueId
        /// </summary>
        /// <param name="venueId">The venue identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/VenueItems/ItemTypes")]
        [ResponseType(typeof(ICollection<OutgoingVenueItemType>))]
        [Authorize]
        public HttpResponseMessage ItemTypeByVenueId(int venueId)
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
                    var itemTypes = unitOfWork.ItemTypes.GetItemTypesByVenueId(currentUserId, venueId);

                    var outgoingVenueItemType = itemTypes.Select(x => OutgoingVenueItemType.Parse(x)).ToList();

                    return JsonFactory.CreateJsonMessage(outgoingVenueItemType, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }

        /// <summary>
        /// Gets all venue item types by venue item type id. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/VenueItems/ItemTypes/ByVenueType")]
        [ResponseType(typeof(ICollection<OutgoingVenueItemType>))]
        [Authorize]
        public HttpResponseMessage GetVenueItemTypes(int venueTypeId)
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
                    var itemTypes = unitOfWork.ItemTypes.GetItemTypesByVenueTypeId(currentUserId, venueTypeId);

                    var outgoingVenueItemType = itemTypes.ToList().Select(x => OutgoingVenueItemType.Parse(x)).ToList();

                    return JsonFactory.CreateJsonMessage(outgoingVenueItemType, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }


        #region Venue Item add edit delete

        /// <summary>
        /// Sets the item type default price.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="price">The price.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/VenueItems/ItemTypes/AddItemPrice")]
        [ResponseType(typeof(OutgoingMessage))]
        [Authorize]
        public HttpResponseMessage SetItemPrice(int itemId, decimal price)
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
                    unitOfWork.ItemTypes.SetItemTypePrice(currentUserId, itemId, price);

                    unitOfWork.Complete();
                }

                return JsonFactory.CreateJsonMessage(new OutgoingMessage { Message = "Added Price for Item", Action = "success" }, HttpStatusCode.OK, this.Request);

            }, this.Request);
        }


        /// <summary>
        /// Adds the venue item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/VenueItems/AddVenueItem")]
        [ResponseType(typeof(OutgoingVenueItems))]
        [Authorize]
        public HttpResponseMessage AddVenueItem(CreateItem item)
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
                    var returnedItem = unitOfWork.VenueItems.CreateItem(userId, item);

                    unitOfWork.Complete();

                    var outgoingItem = OutgoingVenueItems.Parse(returnedItem);

                    return JsonFactory.CreateJsonMessage(outgoingItem, HttpStatusCode.OK, this.Request);
                }


            }, this.Request);
        }

        /// <summary>
        /// Edits the venue item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        [HttpPatch]
        [Route("api/VenueItems/EditVenueItem")]
        [ResponseType(typeof(OutgoingVenueItems))]
        [Authorize]
        public HttpResponseMessage EditVenueItem(IncomingEditItem item)
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
                    var returnedItem = unitOfWork.VenueItems.EditItem(userId, item);

                    unitOfWork.Complete();

                    var outgoingItem = OutgoingVenueItems.Parse(returnedItem);

                    return JsonFactory.CreateJsonMessage(outgoingItem, HttpStatusCode.OK, this.Request);

                }


            }, this.Request);
        }

        //delete venue item

        #endregion

        #region Item Type Add Edit Delete

        /// <summary>
        /// Adds the type of the venue item.
        /// </summary>
        /// <param name="itemType">Type of the item.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("api/VenueItem/AddItemType")]
        [ResponseType(typeof(OutgoingVenueItemType))]
        public HttpResponseMessage AddVenueItemType(CreateItemType itemType)
        {
            return ErrorFactory.Handle(() =>
            {
                var userId = User?.Identity?.GetUserId();

                if (string.IsNullOrWhiteSpace(userId))
                {
                    throw new UnauthorizedAccessException();
                }

                using (var unitOfWork = new UnitOfWork())
                {
                    var addVenueItemType = unitOfWork.ItemTypes.CreateItemType(userId, itemType);

                    var outgoingItemType = OutgoingVenueItemType.Parse(addVenueItemType);

                    unitOfWork.Complete();

                    return JsonFactory.CreateJsonMessage(outgoingItemType, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);

        }

        /// <summary>
        /// Edits the type of the venue item.
        /// </summary>
        /// <param name="editItemType">Type of the edit item.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        [Route("api/VenueItem/EditItemType")]
        [ResponseType(typeof(OutgoingVenueItemType))]
        public HttpResponseMessage EditVenueItemType(IncomingEditItemType editItemType)
        {
            return ErrorFactory.Handle(() =>
            {
                var userId = User?.Identity?.GetUserId();

                if (string.IsNullOrWhiteSpace(userId))
                {
                    throw new UnauthorizedAccessException();
                }

                using (var unitOfWork = new UnitOfWork())
                {
                    var editVenueItemType = unitOfWork.ItemTypes.EditItemType(userId, editItemType);

                    var outgoingItemType = OutgoingVenueItemType.Parse(editVenueItemType);

                    unitOfWork.Complete();

                    return JsonFactory.CreateJsonMessage(outgoingItemType, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);

        }

        //delete venue type
        #endregion
    }
}
