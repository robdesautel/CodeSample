using PoolReservation.Database.Entity.SharedObjects.Repository.Base.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PoolReservation.Database.Entity.Model.Permissions;
using PoolReservation.Database.Entity.Model.ItemType;
using PoolReservation.Database.Entity.Model.ItemType.Incoming;

namespace PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6.Repositories
{
    public class ItemTypeRepository : Repository, IItemTypesRepository
    {
        public ItemTypeRepository(PoolReservationEntities context, UnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }

        public ItemTypes GetItemTypesById(string userId, int id)
        {

            var canView = this.CanUserViewItemType(userId, id);

            if(canView == false)
            {
                throw new UnauthorizedAccessException();
            }


            var itemTypeId = this.dbContext.ItemTypes.FirstOrDefault(x => x.Id == id);
            return itemTypeId;
        }

        public ItemTypes GetItemTypeByVenueItemId(string userId, int itemId)
        {
            var itemTypes = this.unitOfWork.VenueItems.GetVenueItemById(userId, itemId)?.ItemTypes;


            if(itemTypes == null)
            {
                throw new UnauthorizedAccessException();
            }

            var canView = this.CanUserViewItemType(userId, itemTypes?.Id ?? -1);

            if (canView == false)
            {
                throw new UnauthorizedAccessException();
            }

            return itemTypes;
        }

        public IEnumerable<ItemTypes> GetItemTypesByVenueId(string userId, int venueId)
        {
            var canView = this.CanUserViewItemTypes(userId);

            if (canView == false)
            {
                throw new UnauthorizedAccessException();
            }


            var itemtypes = this.dbContext.Venues.Where(x => x.Id == venueId).Take(1).Select(x => x.VenueTypes).SelectMany(x => x.ItemTypes).Distinct();

            return itemtypes;
        }

        public IEnumerable<ItemTypes> GetItemTypesByVenueTypeId(string userId, int venueTypeId)
        {

            var canView = this.CanUserViewItemTypes(userId);

            if (canView == false)
            {
                throw new UnauthorizedAccessException();
            }

            return this.dbContext.ItemTypes.Where(x => x.VenueTypeId == venueTypeId);
        }

        public ItemTypes SetItemTypePrice(string userId, int itemId, decimal price)
        {

            var canEdit = this.CanUserEditItemTypes(userId, itemId);

            if (canEdit == false)
            {
                throw new UnauthorizedAccessException();
            }

            var item = this.dbContext.ItemTypes.FirstOrDefault(x => x.Id == itemId);

            if(item == null)
            {
                throw new Exception("Unable to find the item type.");
            }

            item.Price = price;

            

            return item;
        }

        #region Add Edit Delete

        public ItemTypes CreateItemType(string userId, CreateItemType addItemType)
        {
            var canAdd = this.CanUserAddItemTypes(userId);

            if (canAdd == false)
            {
                throw new UnauthorizedAccessException();
            }

            var newItemType = new ItemTypes
            {
                Id = addItemType.ItemTypeId,
                Name = addItemType.Name,
                VenueTypeId = addItemType.ItemTypeId
            };

            this.dbContext.ItemTypes.Add(newItemType);

            return newItemType;
        }

        public ItemTypes EditItemType(string userId, IncomingEditItemType addItemType)
        {
            var canEdit = this.CanUserEditItemTypes(userId, addItemType.ItemTypeId);

            if (canEdit == false)
            {
                throw new UnauthorizedAccessException();
            }

            var itemType = this.dbContext.ItemTypes.FirstOrDefault(x => x.Id == addItemType.ItemTypeId);

            if (itemType == null)
            {
                throw new UnauthorizedAccessException();
            }

            itemType.Name = addItemType.Name;

            return itemType;
        }

        public void DeleteItemType(string userId, int itemtypeId)
        {
            var canDelete = this.CanUserDeleteItemTypes(userId, itemtypeId);

            if(canDelete == false)
            {
                throw new UnauthorizedAccessException();
            }

            var itemTypes = this.dbContext.ItemTypes.FirstOrDefault(x => x.Id == itemtypeId);

            if (itemTypes == null)
            {
                throw new UnauthorizedAccessException();
            }

            throw new NotImplementedException();
        }



        #endregion

        #region Permissions

        public bool CanUserAddItemTypes(string userId)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            return user?.SitePermissions?.HotelPermissions?.Add ?? false;
        }

        public bool CanUserEditItemTypes(string userId, int itemTypeId)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            return user?.SitePermissions?.HotelPermissions?.Edit ?? false;
        }

        public bool CanUserViewItemType(string userId, int itemTypeId)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            return user?.SitePermissions?.HotelPermissions?.View ?? false;
        }

        public bool CanUserDeleteItemTypes(string userId, int itemTypeId)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            return user?.SitePermissions?.HotelPermissions?.Delete ?? false;
        }

        public bool CanUserViewItemTypes(string userId)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            return user?.SitePermissions?.HotelPermissions?.View ?? false;
        }

        #endregion
    }
}
