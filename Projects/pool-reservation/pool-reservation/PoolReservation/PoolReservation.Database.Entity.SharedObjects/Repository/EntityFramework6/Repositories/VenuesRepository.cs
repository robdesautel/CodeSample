using PoolReservation.Database.Entity.SharedObjects.Repository.Base.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoolReservation.Database.Entity.Model;
using PoolReservation.SharedObjects.Model.Exceptions.Validation;
using PoolReservation.Database.Entity.Model.Venue;
using PoolReservation.Database.Entity.Model.Venue.Incoming;
using System.Data.Entity;

namespace PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6.Repositories
{
    public class VenuesRepository : Repository, IVenuesRepository
    {
        public VenuesRepository(PoolReservationEntities context, UnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }


        public IEnumerable<Venues> GetVenuesByHotelId(string userId, int id, bool includeHidden = false)
        {

            var canView = this.CanUserViewVenues(userId, id);

            if (canView == false)
            {
                throw new UnauthorizedAccessException();
            }

            return this.dbContext.Venues.Where(x => x.HotelId == id && (x.IsHidden == false || x.IsHidden == includeHidden));
        }

        public Venues GetVenueById(string userId, int id)
        {

            var canView = this.CanUserViewVenue(userId, id);

            if (canView == false)
            {
                throw new UnauthorizedAccessException();
            }

            return this.dbContext.Venues?.Include(x => x.VenueTypes)
                .FirstOrDefault(x => x.Id == id);
        }

        #region Add Edit Delete
        public Venues CreateVenue(string userId, CreateVenues addVenue)
        {
            var canAdd = this.CanUserAddVenues(userId, addVenue.HotelId);

            if (canAdd == false)
            {
                throw new UnauthorizedAccessException();
            }


            var newVenue = new Venues
            {

                Name = addVenue.Name,
                HotelId = addVenue.HotelId,
                Type = addVenue.VenueTypeId,
                IsHidden = addVenue.IsHidden
            };

            this.dbContext.Venues.Add(newVenue);

            return newVenue;
        }

        public Venues EditVenue(string userId, IncomingEditVenue addVenue)
        {
            var canEdit = this.CanUserEditVenues(userId, addVenue.Id);

            if (canEdit == false)
            {
                throw new UnauthorizedAccessException();
            }

            var venue = this.dbContext.Venues.FirstOrDefault(x => x.Id == addVenue.Id);

            if (venue == null)
            {
                throw new Exception("Unable to find the incoming venue.");
            }

            venue.Name = addVenue.Name;

            venue.IsHidden = addVenue.IsHidden;

            return venue;
        }

        public void DeleteVenue(string userId, int venueId)
        {
            var canDelete = this.CanUserDeleteVenues(userId, venueId);

            if (canDelete == false)
            {
                throw new UnauthorizedAccessException();
            }

            var venue = this.dbContext.Venues.FirstOrDefault(x => x.Id == venueId);

            if (venue == null)
            {
                throw new UnauthorizedAccessException();
            }

            throw new NotImplementedException();
        }
        #endregion

        #region Permissions
        public bool CanUserAddVenues(string userId, int hotelId)
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

                canAdd = permissions?.HotelPermission?.Add ?? false;
            }

            return canAdd;
        }

        public bool CanUserEditVenues(string userId, int id)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            var canEdit = false;

            if (user?.SitePermissions?.HotelPermissions?.Edit == true)
            {
                canEdit = true;
            }
            else
            {
                var hotelId = this.dbContext.Venues?.Include(x => x.Hotels.Id)?.FirstOrDefault(x => x.Id == id)?.Hotels?.Id;

                if (hotelId != null)
                {
                    var permissions = this.unitOfWork.Hotels.GetPermissionsForUserAtHotel(userId, hotelId ?? -1);

                    canEdit = permissions?.ItemPermissions?.Edit ?? false;
                }
            }

            return canEdit;
        }

        public bool CanUserViewVenue(string userId, int id)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            var canView = false;

            if (user?.SitePermissions?.HotelPermissions?.View == true)
            {
                canView = true;
            }
            else
            {
                var hotelId = this.dbContext.Venues?.Include(x => x.Hotels.Id)?.FirstOrDefault(x => x.Id == id)?.Hotels?.Id;

                if (hotelId != null)
                {
                    var permissions = this.unitOfWork.Hotels.GetPermissionsForUserAtHotel(userId, hotelId ?? -1);

                    canView = permissions?.ItemPermissions?.View ?? false;
                }
            }

            return canView;
        }

        public bool CanUserDeleteVenues(string userId, int id)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            var canDelete = false;

            if (user?.SitePermissions?.HotelPermissions?.Delete == true)
            {
                canDelete = true;
            }
            else
            {
                var hotelId = this.dbContext.Venues?.Include(x => x.Hotels.Id)?.FirstOrDefault(x => x.Id == id)?.Hotels?.Id;

                if (hotelId != null)
                {
                    var permissions = this.unitOfWork.Hotels.GetPermissionsForUserAtHotel(userId, hotelId ?? -1);

                    canDelete = permissions?.ItemPermissions?.Delete ?? false;
                }
            }

            return canDelete;
        }

        public bool CanUserViewVenues(string userId, int hotelId)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            var canView = false;

            if (user?.SitePermissions?.HotelPermissions?.View == true)
            {
                canView = true;
            }
            else
            {

                var permissions = this.unitOfWork.Hotels.GetPermissionsForUserAtHotel(userId, hotelId);

                canView = permissions?.ItemPermissions?.View ?? false;

            }

            return canView;
        }
        #endregion

        #region Blackout

        public IEnumerable<Blackout> GetBlackoutsForVenue(string userId, int venueId, DateTime startDate, DateTime endDate)
        {

            var canView = this.CanUserViewVenue(userId, venueId);

            if (canView == false)
            {
                throw new UnauthorizedAccessException();
            }

            if (startDate > endDate)
            {
                throw new InvalidModelException("Start date cannot be greater than end date.");
            }

            return this.dbContext.Calendar.Where(x => x.VenueId == venueId).Where(blackoutEvent => (startDate >= blackoutEvent.StartDate && startDate <= blackoutEvent.EndDate) || (endDate >= blackoutEvent.StartDate && endDate <= blackoutEvent.EndDate) || (startDate <= blackoutEvent.StartDate && endDate >= blackoutEvent.EndDate)).ToList().Select(blackoutEvent => new Blackout { Id = blackoutEvent.Id, StartDate = blackoutEvent.StartDate, EndDate = blackoutEvent.EndDate, VenueId = venueId });
        }


        public void DeleteBlackout(string userId, int blackoutID)
        {
            var blackout = this.dbContext.Calendar.FirstOrDefault(x => x.Id == blackoutID);

            if (blackout == null)
            {
                throw new UnauthorizedAccessException(); //Do not let them know it does not exist.
            }

            var canEdit = this.CanUserEditVenues(userId, blackout.VenueId);

            if (canEdit == false)
            {
                throw new UnauthorizedAccessException();
            }

            this.dbContext.Calendar.Remove(blackout);
        }

        public Blackout GetBlackout(string userId, int blackoutId)
        {
            var item = this.dbContext.Calendar.FirstOrDefault(x => x.Id == blackoutId);

            if (item == null)
            {
                throw new UnauthorizedAccessException();
            }

            var canView = this.CanUserViewVenue(userId, item.VenueId);

            if (canView == false)
            {
                throw new UnauthorizedAccessException();
            }

            var returnItem = new Blackout
            {
                Id = item.Id,
                EndDate = item.EndDate,
                StartDate = item.StartDate,
                VenueId = item.VenueId
            };

            return returnItem;
        }

        public Blackout EditBlackout(string userId, Blackout blackout)
        {
            if (blackout == null || blackout.StartDate > blackout.EndDate)
            {
                throw new Exception();
            }


            var item = this.dbContext.Calendar.FirstOrDefault(x => x.Id == blackout.Id);

            if (item == null)
            {
                throw new UnauthorizedAccessException();
            }

            var canEdit = this.CanUserEditVenues(userId, item.VenueId);

            if (canEdit == false)
            {
                throw new UnauthorizedAccessException();
            }


            item.StartDate = blackout.StartDate;
            item.EndDate = blackout.EndDate;

            var returnItem = new Blackout
            {
                Id = item.Id,
                EndDate = item.EndDate,
                StartDate = item.StartDate,
                VenueId = item.VenueId
            };

            return returnItem;
        }

        public Calendar CreateBlackout(string userId, Blackout blackout)
        {
            if (blackout == null || blackout.StartDate > blackout.EndDate)
            {
                throw new Exception();
            }


            var canEdit = this.CanUserEditVenues(userId, blackout.VenueId);

            if (canEdit == false)
            {
                throw new UnauthorizedAccessException();
            }

            var blackoutToAdd = new Calendar()
            {
                StartDate = blackout.StartDate,
                EndDate = blackout.EndDate,
                VenueId = blackout.VenueId
            };

            this.dbContext.Calendar.Add(blackoutToAdd);

            return blackoutToAdd;
        }

        #endregion


    }

}
