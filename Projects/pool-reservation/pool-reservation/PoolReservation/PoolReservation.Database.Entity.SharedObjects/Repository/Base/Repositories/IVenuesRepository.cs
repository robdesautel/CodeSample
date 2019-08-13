using PoolReservation.Database.Entity.Model;
using PoolReservation.Database.Entity.Model.Venue;
using PoolReservation.Database.Entity.Model.Venue.Incoming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.SharedObjects.Repository.Base.Repositories
{
    public interface IVenuesRepository : IRepository
    {
        bool CanUserAddVenues(string userId, int hotelId);
        bool CanUserEditVenues(string userId, int id);
        bool CanUserViewVenue(string userId, int id);
        bool CanUserViewVenues(string userId, int hotelId);
        bool CanUserDeleteVenues(string userId, int id);

        Venues GetVenueById(string userId, int id);

        IEnumerable<Venues> GetVenuesByHotelId(string userId, int id, bool includeHidden = false);

        IEnumerable<Blackout> GetBlackoutsForVenue(string userId, int venueId, DateTime startDate, DateTime endDate);

        Venues CreateVenue(string userId, CreateVenues addVenue);

        Venues EditVenue(string userId, IncomingEditVenue addVenue);

        void DeleteVenue(string userId, int venueId);

        void DeleteBlackout(string userId, int blackoutId);

        Blackout GetBlackout(string userId, int blackoutId);

        Blackout EditBlackout(string userId, Blackout blackout);

        Calendar CreateBlackout(string userId, Blackout blackout);
    }
}
