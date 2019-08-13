using PoolReservation.Database.Entity.SharedObjects.Repository.Base.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoolReservation.Database.Entity.Model;
using PoolReservation.SharedObjects.Model.Exceptions.Validation;
using PoolReservation.Database.Entity.Model.VenueType;
using PoolReservation.Database.Entity.Model.VenueType.IncomingVenueType;
using System.Data.Entity;

namespace PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6.Repositories
{
    public class VenueTypesRepository : Repository, IVenueTypesRepository
    {

        public VenueTypesRepository(PoolReservationEntities context, UnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }
        #region permissions
        public bool CanUserAddVenueTypes(string userId)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            return user?.SitePermissions?.HotelPermissions?.Add ?? false;
        }

        public bool CanUserDeleteVenueTypes(string userId)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            return user?.SitePermissions?.HotelPermissions?.Delete ?? false;
        }

        public bool CanUserEditVenueTypes(string userId)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            return user?.SitePermissions?.HotelPermissions?.Edit ?? false;
        }

        public bool CanUserViewVenueTypes(string userId)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            return user?.SitePermissions?.HotelPermissions?.View ?? false;
        }

        #endregion

        public IEnumerable<VenueTypes> GetVenueTypes(string userId)
        {

            var canView = this.CanUserViewVenueTypes(userId);

            if(canView == false)
            {
                throw new UnauthorizedAccessException();
            }

            return this.dbContext.VenueTypes;
        }

        public List<VenueTypes> GetVenueTypesByHotelId(string userId, int hotelId)
        {

            var canView = this.CanUserViewVenueTypes(userId);

            if (canView == false)
            {
                throw new UnauthorizedAccessException();
            }

            canView = this.unitOfWork.Venues.CanUserViewVenues(userId, hotelId);

            if (canView == false)
            {
                throw new UnauthorizedAccessException();
            }

            var venueTypes = this.dbContext.Venues.Where(x => x.HotelId == hotelId).Select(y => y.VenueTypes).ToList();
            return venueTypes;

        }

        public VenueTypes GetVenueTypesById(string userId, int id)
        {

            var canView = this.CanUserViewVenueTypes(userId);

            if (canView == false)
            {
                throw new UnauthorizedAccessException();
            }

            var venueType = this.dbContext.VenueTypes.FirstOrDefault(x => x.Id == id);
            return venueType;
        }

        public VenueTypes GetVenueTypesByVenueId(string userId, int venueID)
        {
            var canView = this.CanUserViewVenueTypes(userId);

            if (canView == false)
            {
                throw new UnauthorizedAccessException();
            }

            canView = this.unitOfWork.Venues.CanUserViewVenue(userId, venueID);

            if (canView == false)
            {
                throw new UnauthorizedAccessException();
            }

            var venueTypes = this.dbContext.Venues.Include(x => x.VenueTypes).FirstOrDefault(x => x.Id == venueID)?.VenueTypes;

            return venueTypes;
        }

        public VenueTypes AddVenueType(string userId, CreateVenueType addVenueType)
        {
            var canAdd = this.CanUserAddVenueTypes(userId);

            if (canAdd == false)
            {
                throw new UnauthorizedAccessException();
            }

            var newVenueType = new VenueTypes
            {
                Name = addVenueType.Name
            };

            this.dbContext.VenueTypes.Add(newVenueType);

            return newVenueType;
        }

        public VenueTypes EditVenueType(string userId, IncomingEditVenueType addVenueType)
        {
            var canEdit = this.CanUserEditVenueTypes(userId);

            if (canEdit == false)
            {
                throw new UnauthorizedAccessException();
            }

            var venueType = this.dbContext.VenueTypes.FirstOrDefault(x => x.Id == addVenueType.Id);

            if (venueType == null)
            {
                throw new UnauthorizedAccessException();

            }

            venueType.Name = addVenueType.Name;

            return venueType;
        }

        public void DeleteVenueType(string userId, int venueTypeId)
        {
            var canDelete = this.CanUserDeleteVenueTypes(userId);

            if (canDelete == false)
            {
                throw new UnauthorizedAccessException();
            }

            var venueType = this.dbContext.VenueTypes.FirstOrDefault(x => x.Id == venueTypeId);

            if (venueType == null)
            {
                throw new UnauthorizedAccessException();
            }

            throw new NotImplementedException();

        }
    }
}

