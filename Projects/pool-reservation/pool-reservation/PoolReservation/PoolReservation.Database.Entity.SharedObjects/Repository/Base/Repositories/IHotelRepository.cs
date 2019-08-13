using PoolReservation.Database.Entity.Model;
using PoolReservation.Database.Entity.Model.Hotel;
using PoolReservation.Database.Entity.Model.Hotel.Incoming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.SharedObjects.Repository.Base.Repositories
{
    public interface IHotelRepository : IRepository
    {
        bool CanUserAddHotel(string userId);
        bool CanUserEditHotelDetails(string userId, int hotelId);
        bool CanUserEditHotelSiteAdmin(string userId, int hotelId);
        bool CanUserViewHotel(string userId, int hotelId);
        bool CanUserViewHotels(string userId);
        bool CanUserDeleteHotelDetails(string userId, int hotelId);
        bool CanUserDeleteHotelSiteAdmin(string userId, int hotelId);

        bool CanUserAddOtherUsers(string userId, int hotelId);
        bool CanUserEditOtherUsers(string userId, int hotelId);
        bool CanUserViewOtherUsers(string userId, int hotelId);
        bool CanUserDeleteOtherUsers(string userId, int hotelId);


        bool IsUserSitewideHotelAdmin(string userId);

        Hotels GetHotelById(string userId, int id);

        IEnumerable<Hotels> GetHotelsInRadius(string userId, LatLonLocation bottomLeftPoint, LatLonLocation topRightPoint, double radiusInKilometers);

        IEnumerable<Hotels> GetHotelsByUserId(string userId);

        Hotels CreateHotel(string  userId, CreateHotels hotelToAdd);

        Hotels EditHotel(string userId, IncomingEditHotel hotelToAdd);

        HotelPermissions GetPermissionsForUserAtHotel(string userId, int hotelId);

        IEnumerable<Hotels> SearchHotels(string userId, string query, int? startingIndex, int? numberToGet, bool includeHidden = false);

        IEnumerable<HotelUsers> SearchHotelUsers(string userId, int hotelId, string query, int startingIndex, int numberToGet);

        IEnumerable<HotelUsers> GetHotelUsers(string userId, int hotelId, int startingIndex, int numberToGet);

        HotelUsers GetHotelUser(string userId, int hotelId, string userIdToGet);

        HotelUsers AddUserPermissionsInHotel(string currentUserId, string userIdToChange, int hotelId, int permissionsId);

        IEnumerable<HotelPermissions> GetHotelPermissions();

        HotelUsers ChangeUserPermissionsInHotel(string currentUserId, int hotelId, string userIdToChange, int permissionsId);

        void DeleteHotels(string userId, int hotelId);

        Hotels SetHotelImageADMIN(string userId, int hotelId, Guid pictureId);
    }
}
