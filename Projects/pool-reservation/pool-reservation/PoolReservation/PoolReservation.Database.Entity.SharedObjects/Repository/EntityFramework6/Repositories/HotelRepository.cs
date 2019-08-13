using PoolReservation.Database.Entity.Model;
using PoolReservation.Database.Entity.SharedObjects.Repository.Base.Repositories;
using PoolReservation.Database.Entity.Model.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoolReservation.Database.Entity.Model.Permissions;
using PoolReservation.Database.Entity.Model.Hotel.Incoming;
using PoolReservation.SharedObjects.Model.Exceptions.Validation;
using System.Data.Entity;

namespace PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6.Repositories
{
    public class HotelRepository : Repository, IHotelRepository
    {
        public HotelRepository(PoolReservationEntities context, UnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }

        public Hotels GetHotelById(string userId, int id)
        {

            var canView = this.CanUserViewHotel(userId, id);

            if(canView == false)
            {
                throw new UnauthorizedAccessException();
            }

            return this.dbContext.Hotels.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Hotels> GetHotelsByUserId(string id)
        {
            return this.dbContext.HotelUsers.Where(x => x.UserId == id).Select(x => x.Hotels);
        }

        public IEnumerable<Hotels> GetHotelsInRadius(string userId, LatLonLocation bottomLeftPoint, LatLonLocation topRightPoint, double radiusInKilometers)
        {
            var canView = this.CanUserViewHotels(userId);

            if (canView == false)
            {
                throw new UnauthorizedAccessException();
            }


            var hotelsToReturn = this.dbContext.Hotels.Where(hotel => hotel.IsHidden == false && hotel.Latitude >= bottomLeftPoint.Latitude && hotel.Longitude >= bottomLeftPoint.Longitude
                                                                   && hotel.Latitude <= topRightPoint.Latitude && hotel.Longitude <= topRightPoint.Longitude);

            return hotelsToReturn;
        }

        #region Add Edit Delete
        public Hotels CreateHotel(string userId, CreateHotels hotelToAdd)
        {
            var user = this.unitOfWork.Users.GetUserById(userId, userId);

            if (user == null)
            {
                throw new Exception("Unable to find user");
            }

            var newHotel = new Hotels
            {

                Name = hotelToAdd.Name,
                Address = hotelToAdd.Address,
                TaxRate = hotelToAdd.TaxRate,
                Latitude = hotelToAdd.Latitude,
                Longitude = hotelToAdd.Longitude,
                IsHidden = hotelToAdd.IsHidden
            };

            var newHotelUserPermissions = new HotelUsers
            {
                UserId = user.Id,
                PermissionId = Convert.ToInt32(HotelUsersPermissionsEnum.FULL_ADMIN)
            };

            newHotel.HotelUsers.Add(newHotelUserPermissions);

            this.dbContext.Hotels.Add(newHotel);

            return newHotel;
        }

        public Hotels EditHotel(string userId, IncomingEditHotel hotelToEdit)
        {
            var hotel = this.dbContext.Hotels.FirstOrDefault(x => x.Id == hotelToEdit.Id);

            if (hotel == null)
            {
                throw new Exception("Hotel does not exist.");
            }

            var auth = this.CanUserEditHotelSiteAdmin(userId, hotel.Id);

            if(auth == false)
            {
                throw new UnauthorizedAccessException();
            }

            hotel.Name = hotelToEdit.Name;
            hotel.Address = hotelToEdit.Address;
            hotel.TaxRate = hotelToEdit.TaxRate;
            hotel.Latitude = hotelToEdit.Latitude;
            hotel.Longitude = hotelToEdit.Longitude;
            hotel.IsHidden = hotelToEdit.IsHidden;

            return hotel;
        }

        public void DeleteHotels(string userId, int hotelId)
        {
            var canDelete = this.CanUserDeleteHotelSiteAdmin(userId, hotelId);
            if (canDelete == false)
            {
                throw new UnauthorizedAccessException();
            }

            var hotel = this.dbContext.Hotels.FirstOrDefault(x => x.Id == hotelId);

            if (hotel == null)
            {
                throw new UnauthorizedAccessException();
            }

            throw new NotImplementedException();
        }
        #endregion

        #region Permissions
        public bool CanUserAddHotel(string userId)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            return user?.SitePermissions?.HotelPermissions?.Add ?? false;
        }

        public bool CanUserEditHotelDetails(string userId, int hotelId)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            var canEdit = false;

            if (user?.SitePermissions?.HotelPermissions?.Edit == true)
            {
                canEdit = true;
            }
            else
            {
                var permissions = user?.HotelUsers?.FirstOrDefault(x => x.HotelId == hotelId)?.HotelPermissions;

                canEdit = permissions?.HotelPermission?.Edit ?? false;
            }

            return canEdit;
        }

        public bool CanUserEditHotelSiteAdmin(string userId, int hotelId)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            var canEdit = false;

            if (user?.SitePermissions?.HotelPermissions?.Edit == true)
            {
                canEdit = true;
            }

            return canEdit;
        }

        public bool CanUserViewHotel(string userId, int hotelId)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            var canView = false;

            if (user?.SitePermissions?.HotelPermissions?.View == true)
            {
                canView = true;
            }
            else
            {
                var permissions = user?.HotelUsers?.FirstOrDefault(x => x.HotelId == hotelId)?.HotelPermissions;

                canView = permissions?.HotelPermission?.View ?? false;
            }

            return canView;
        }

        public bool CanUserDeleteHotelDetails(string userId, int hotelId)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            var canDelete = false;

            if (user?.SitePermissions?.HotelPermissions?.Delete == true)
            {
                canDelete = true;
            }
            else
            {
                var permissions = user?.HotelUsers?.FirstOrDefault(x => x.HotelId == hotelId)?.HotelPermissions;

                canDelete = permissions?.UserPermissions?.Delete ?? false;
            }

            return canDelete;
        }

        public bool CanUserDeleteHotelSiteAdmin(string userId, int hotelId)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            var canDelete = false;

            if (user?.SitePermissions?.HotelPermissions?.Delete == true)
            {
                canDelete = true;
            }

            return canDelete;
        }


        public bool CanUserAddOtherUsers(string userId, int hotelId)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            var canAdd = false;

            if (user?.SitePermissions?.OtherUsersPermissions?.Add == true)
            {

                canAdd = true;
            }
            else
            {
                var permissions = user?.HotelUsers?.FirstOrDefault(x => x.HotelId == hotelId)?.HotelPermissions;

                canAdd = permissions?.UserPermissions?.Add ?? false;
            }

            return canAdd;
        }

        public bool CanUserEditOtherUsers(string userId, int hotelId)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            var canEdit = false;

            if (user?.SitePermissions?.OtherUsersPermissions?.Edit == true)
            {

                canEdit = true;
            }
            else
            {
                var permissions = user?.HotelUsers?.FirstOrDefault(x => x.HotelId == hotelId)?.HotelPermissions;

                canEdit = permissions?.UserPermissions?.Edit ?? false;
            }

            return canEdit;
        }

        public bool CanUserViewOtherUsers(string userId, int hotelId)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            var canView = false;

            if (user?.SitePermissions?.OtherUsersPermissions?.View == true)
            {

                canView = true;
            }
            else
            {
                var permissions = user?.HotelUsers?.FirstOrDefault(x => x.HotelId == hotelId)?.HotelPermissions;

                canView = permissions?.UserPermissions?.View ?? false;
            }

            return canView;
        }

        public bool CanUserDeleteOtherUsers(string userId, int hotelId)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            var canDelete = false;

            if (user?.SitePermissions?.OtherUsersPermissions?.Delete == true)
            {

                canDelete = true;
            }
            else
            {
                var permissions = user?.HotelUsers?.FirstOrDefault(x => x.HotelId == hotelId)?.HotelPermissions;

                canDelete = permissions?.UserPermissions?.Delete ?? false;
            }

            return canDelete;
        }

        #endregion

        #region Hotel Permissions
        public HotelPermissions GetPermissionsForUserAtHotel(string userId, int hotelId)
        {
            return this.dbContext.HotelUsers.Include(x => x.HotelPermissions).FirstOrDefault(x => x.UserId == userId && x.HotelId == hotelId)?.HotelPermissions;
        }

        public bool IsUserSitewideHotelAdmin(string userId)
        {
            var perms = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId)?.SitePermissions;

            if (perms == null)
            {
                return false;
            }

            var isAdmin = perms.HotelPermissions.Add && perms.HotelPermissions.Edit && perms.HotelPermissions.View && perms.HotelPermissions.Delete;

            return isAdmin;
        }

        public IEnumerable<Hotels> SearchHotels(string userId, string query, int? startingIndex, int? numberToGet, bool includeHidden = false)
        {
            var canView = this.CanUserViewHotels(userId);

            if (!canView)
            {
                throw new UnauthorizedAccessException();
            }

            var isAdmin = this.IsUserSitewideHotelAdmin(userId);

            var fixedQuery = query?.Trim().ToLower() ?? "";

            IEnumerable<Hotels> hotels = null;

            if (isAdmin)
            {
                hotels =  this.dbContext.Hotels.Where(hotel => (hotel.Name.ToLower().Contains(fixedQuery) || hotel.Id.ToString().Contains(query)) && (hotel.IsHidden == false || hotel.IsHidden == includeHidden)).OrderBy(x => x.Id);
            }
            else
            {
                //Hide hidden hotels from the hotel manager and the hotel users so that they cannot do anything to it. 
                hotels =  this.dbContext.HotelUsers.Where(x => x.UserId == userId).Select(x => x.Hotels).Where(hotel => (hotel.Name.ToLower().Contains(fixedQuery) || hotel.Id.ToString().Contains(query)) && hotel.IsHidden == false).OrderBy(x => x.Id);
            }

            if(startingIndex != null && numberToGet != null)
            {
                hotels = hotels.Skip(startingIndex ?? -1).Take(numberToGet ?? -1);
            }

            return hotels;


        }

        public bool CanUserViewHotels(string userId)
        {
            var perms = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId)?.SitePermissions;

            if (perms == null)
            {
                return false;
            }

            return perms.HotelPermissions.View;
        }

        public IEnumerable<HotelUsers> SearchHotelUsers(string userId, int hotelId, string query, int startingIndex, int numberToGet)
        {
            var canView = this.CanUserViewOtherUsers(userId, hotelId);

            if (!canView)
            {
                throw new UnauthorizedAccessException();
            }

            var smallQuery = query.Trim().ToLower();

            var users = this.dbContext.HotelUsers.Include(x => x.AspNetUsers).Include(x => x.HotelPermissions).Where(x => x.HotelId == hotelId).Where(x => x.UserId.ToLower().Contains(smallQuery) || (x.AspNetUsers.FirstName + " " + x.AspNetUsers.LastName).ToLower().Contains(smallQuery));


            var smallerUsers = users.OrderBy(x => x.AspNetUsers.Id).Skip(startingIndex).Take(numberToGet);
            return users;
        }

        
        public IEnumerable<HotelUsers> GetHotelUsers(string userId, int hotelId, int startingIndex, int numberToGet)
        {
            var canView = this.CanUserViewOtherUsers(userId, hotelId);

            if (!canView)
            {
                throw new UnauthorizedAccessException();
            }

            var users = this.dbContext.HotelUsers.Include(x => x.AspNetUsers).Include(x => x.HotelPermissions).Where(x => x.HotelId == hotelId);


            var smallerUsers = users.OrderBy(x => x.AspNetUsers.Id).Skip(startingIndex).Take(numberToGet);
            return users;
        }

        public HotelUsers GetHotelUser(string userId, int hotelId, string userIdToGet)
        {
            var canView = this.CanUserViewOtherUsers(userId, hotelId);

            if (!canView)
            {
                throw new UnauthorizedAccessException();
            }

            return this.dbContext.HotelUsers.Include(x => x.AspNetUsers).Include(x => x.HotelPermissions).FirstOrDefault(x => x.HotelId == hotelId || x.UserId == userId);
        }

        public IEnumerable<HotelPermissions> GetHotelPermissions()
        {
            return this.dbContext.HotelPermissions;
        }

        public HotelUsers ChangeUserPermissionsInHotel(string currentUserId, int hotelId, string userIdToChange, int permissionsId)
        {
            var canEdit = this.CanUserEditOtherUsers(currentUserId, hotelId);

            if (!canEdit)
            {
                throw new UnauthorizedAccessException();
            }

            var user = this.dbContext.HotelUsers.FirstOrDefault(x => x.HotelId == hotelId && x.UserId == userIdToChange);

            user.PermissionId = permissionsId;

            return user;
        }

        public Hotels SetHotelImageADMIN(string userId, int hotelId, Guid pictureId)
        {
            var auth = this.CanUserEditHotelDetails(userId, hotelId);

            if(auth == false)
            {
                throw new UnauthorizedAccessException();
            }

            var hotel = this.GetHotelById(userId, hotelId);

            if(hotel == null)
            {
                throw new Exception();
            }

            var picture = this.unitOfWork.Pictures.GetPicture(userId, pictureId);

            if (picture == null)
            {
                throw new Exception("Unable to find the picture");
            }

            hotel.PictureId = picture.Id;

            return hotel;
        }

        public HotelUsers AddUserPermissionsInHotel(string currentUserId, string userIdToChange, int hotelId,  int permissionsId)
        {
            var canEdit = this.CanUserEditOtherUsers(currentUserId, hotelId);

            if (!canEdit)
            {
                throw new UnauthorizedAccessException();
            }

            var user = this.dbContext.HotelUsers.FirstOrDefault(x => x.HotelId == hotelId && x.UserId == userIdToChange);

            if(user != null)
            {
                throw new Exception();
            }

            var newUser = new HotelUsers
            {
                UserId = userIdToChange,
                HotelId = hotelId,
                PermissionId = permissionsId
            };

            this.dbContext.HotelUsers.Add(newUser);

            return newUser;
        }




        #endregion


    }

}
