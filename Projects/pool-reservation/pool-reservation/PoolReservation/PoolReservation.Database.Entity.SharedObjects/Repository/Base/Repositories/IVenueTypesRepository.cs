using PoolReservation.Database.Entity.Model.VenueType;
using PoolReservation.Database.Entity.Model.VenueType.IncomingVenueType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.SharedObjects.Repository.Base.Repositories
{
   public interface IVenueTypesRepository : IRepository
    {
        bool CanUserAddVenueTypes(string userId);
        bool CanUserEditVenueTypes(string userId);
        bool CanUserViewVenueTypes(string userId);
        bool CanUserDeleteVenueTypes(string userId);

        VenueTypes GetVenueTypesById(string userId, int id);

        VenueTypes GetVenueTypesByVenueId(string userId, int venueID);

        List<VenueTypes> GetVenueTypesByHotelId(string userId, int hotelId);

        VenueTypes AddVenueType(string userId, CreateVenueType addVenueType);

        VenueTypes EditVenueType(string userId, IncomingEditVenueType addVenueType);

        void DeleteVenueType(string userId, int venueTypeId);

        IEnumerable<VenueTypes> GetVenueTypes(string userId);

    }
}
