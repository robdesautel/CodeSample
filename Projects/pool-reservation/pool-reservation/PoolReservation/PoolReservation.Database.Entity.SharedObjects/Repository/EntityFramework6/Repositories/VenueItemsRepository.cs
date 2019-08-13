using PoolReservation.Database.Entity.SharedObjects.Repository.Base.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PoolReservation.Database.Entity.Model.Item;
using PoolReservation.Database.Entity.Model.Item.Incoming;
using PoolReservation.SharedObjects.Model.Exceptions.Validation;

namespace PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6.Repositories
{
    public class ItemsRepository : Repository, IVenueItemsRepository
    {
        public ItemsRepository(PoolReservationEntities context, UnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }

        public IEnumerable<VenueItems> GetVenueItemsByVenueId(string userId, int venueId, bool includeHidden = false)
        {

            var canView = this.CanUserViewVenueItems(userId, venueId);

            if(canView == false)
            {
                throw new UnauthorizedAccessException();
            }

            return this.dbContext.VenueItems
                .Include(x => x.ItemTypes)
                .Include(x => x.Icons)
                .Include(x => x.Icons.Pictures)
                .Include(x => x.Icons.Pictures.PictureResolutions)
                .Include(x => x.Icons.Pictures.PictureResolutions.Select(y => y.PictureUrls))
                .Where(x => x.VenueId == venueId && (x.IsHidden == false || x.IsHidden == includeHidden));
        }


        public VenueItems GetVenueItemById(string userId, int id)
        {
            var canView = this.CanUserViewVenueItem(userId, id);

            if (canView == false)
            {
                throw new UnauthorizedAccessException();
            }

            return this.dbContext.VenueItems
                .Include(x => x.ItemTypes)
                .Include(x => x.Icons)
                .Include(x => x.Icons.Pictures)
                .Include(x => x.Icons.Pictures.PictureResolutions)
                .Include(x => x.Icons.Pictures.PictureResolutions.Select(y => y.PictureUrls))
                .FirstOrDefault(x => x.Id == id);
        }

        public VenueItemWithQuantity GetVenueItemWithQuantityById(string userId, int id)
        {
            var canView = this.CanUserViewVenueItem(userId, id);

            if (canView == false)
            {
                throw new UnauthorizedAccessException();
            }

            return this.dbContext.VenueItemWithQuantity.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<VenueItemWithQuantity> GetVenueItemsQuantityByVenueId(string userId, int venueId)
        {
            var canView = this.CanUserViewVenueItems(userId, venueId);

            if (canView == false)
            {
                throw new UnauthorizedAccessException();
            }

            return this.dbContext.VenueItemWithQuantity.Where(x => x.VenueId == venueId);
        }

        public VenueItems CreateItem(string userId, CreateItem addItem)
        {
            var venueId = addItem.VenueId;

            var hotelId = this.unitOfWork.Venues.GetVenueById(userId, venueId)?.HotelId;

            if(hotelId == null)
            {
                throw new UnauthorizedAccessException();
            }

            var canAdd = this.CanUserAddVenueItems(userId, hotelId ?? -1);

            if(canAdd == false)
            {
                throw new UnauthorizedAccessException();
            }

            var newItem = new VenueItems
            {
                Name = addItem.Name,
                Type = addItem.ItemTypeId,
                VenueId = addItem.VenueId,
                Price = addItem.Price,
                IconId = addItem.IconId,
                IsHidden = addItem.IsHidden
            };

            var quantityItem = new VenueItemQuantity
            {
                DateEffective = DateTime.UtcNow.Date,
                Quantity = addItem.NormalQuantity
            };

            newItem.VenueItemQuantity.Add(quantityItem);

            this.dbContext.VenueItems.Add(newItem);

            return newItem;
        }

        public VenueItems EditItem(string userId, IncomingEditItem editItem)
        {

            var canEdit = this.CanUserEditVenueItems(userId, editItem.Id);

            if(canEdit == false)
            {
                throw new UnauthorizedAccessException();
            }

            var item = this.dbContext.VenueItems.FirstOrDefault(x => x.Id == editItem.Id);

            if(item == null)
            {
                throw new UnauthorizedAccessException();
            }

            var permissions = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId)?.SitePermissions;



            item.Name = editItem.Name;
            item.IconId = editItem.IconId;
            item.IsHidden = editItem.IsHidden;

            if (permissions?.PricePermissions?.Edit == true)
            {
                item.Price = editItem.Price;
            }

            return item;
        }

        public bool CanUserAddVenueItems(string userId, int hotelId)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            var canAdd = false;

            if (user?.SitePermissions?.HotelPermissions?.Add == true)
            {
                canAdd = true;
            }
            else
            {
                var permissions = this.unitOfWork.Hotels.GetPermissionsForUserAtHotel(userId, hotelId);

                canAdd = permissions?.ItemPermissions?.Add ?? false;
            }

            return canAdd;
        }

        public bool CanUserEditVenueItems(string userId, int id)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            var canEdit = false;

            if (user?.SitePermissions?.HotelPermissions?.Edit == true)
            {
                canEdit = true;
            }
            else
            {
                var hotelId = this.dbContext.VenueItems?.Include(x => x.Venues.Hotels.Id)?.FirstOrDefault(x => x.Id == id)?.Venues?.Hotels?.Id;

                if(hotelId != null)
                {
                    var permissions = this.unitOfWork.Hotels.GetPermissionsForUserAtHotel(userId, hotelId ?? -1);

                    canEdit = permissions?.ItemPermissions?.Edit ?? false;
                }
            }

            return canEdit;
        }

        public bool CanUserViewVenueItem(string userId, int id)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            var canView = false;

            if (user?.SitePermissions?.HotelPermissions?.View == true)
            {
                canView = true;
            }
            else
            {
                var hotelId = this.dbContext.VenueItems?.Include(x => x.Venues.Hotels.Id)?.FirstOrDefault(x => x.Id == id)?.Venues?.Hotels?.Id;

                if (hotelId != null)
                {
                    var permissions = this.unitOfWork.Hotels.GetPermissionsForUserAtHotel(userId, hotelId ?? -1);

                    canView = permissions?.ItemPermissions?.View ?? false;
                }
            }

            return canView;
        }

        public bool CanUserDeleteVenueItems(string userId, int id)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            var canDelete = false;

            if (user?.SitePermissions?.HotelPermissions?.Delete == true)
            {
                canDelete = true;
            }
            else
            {
                var hotelId = this.dbContext.VenueItems?.Include(x => x.Venues.Hotels.Id)?.FirstOrDefault(x => x.Id == id)?.Venues?.Hotels?.Id;

                if (hotelId != null)
                {
                    var permissions = this.unitOfWork.Hotels.GetPermissionsForUserAtHotel(userId, hotelId ?? -1);

                    canDelete = permissions?.ItemPermissions?.Delete ?? false;
                }
            }

            return canDelete;
        }

        public bool CanUserViewVenueItems(string userId, int venueId)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            var canView = false;

            if (user?.SitePermissions?.HotelPermissions?.View == true)
            {
                canView = true;
            }
            else
            {
                var hotelId = this.dbContext.Venues?.Include(x => x.Hotels.Id)?.FirstOrDefault(x => x.Id == venueId)?.Hotels?.Id;

                if (hotelId != null)
                {
                    var permissions = this.unitOfWork.Hotels.GetPermissionsForUserAtHotel(userId, hotelId ?? -1);

                    canView = permissions?.ItemPermissions?.View ?? false;
                }
            }

            return canView;
        }


    }
    
}
