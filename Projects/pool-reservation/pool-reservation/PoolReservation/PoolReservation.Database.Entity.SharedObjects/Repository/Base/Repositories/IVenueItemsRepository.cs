using PoolReservation.Database.Entity.Model.Item;
using PoolReservation.Database.Entity.Model.Item.Incoming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.SharedObjects.Repository.Base.Repositories
{
    public interface IVenueItemsRepository : IRepository
    {
        bool CanUserAddVenueItems(string userId, int hotelId);
        bool CanUserEditVenueItems(string userId, int id);
        bool CanUserViewVenueItem(string userId, int id);
        bool CanUserViewVenueItems(string userId, int venueId);
        bool CanUserDeleteVenueItems(string userId, int id);

        VenueItems GetVenueItemById(string userId, int id);

        IEnumerable<VenueItems> GetVenueItemsByVenueId(string userId, int venueId, bool includeHidden = false);

        VenueItemWithQuantity GetVenueItemWithQuantityById(string userId, int id);

        IEnumerable<VenueItemWithQuantity> GetVenueItemsQuantityByVenueId(string userId, int venueId);

        VenueItems CreateItem(string userId, CreateItem addItem);

        VenueItems EditItem(string userId, IncomingEditItem addItem);

    }
}
