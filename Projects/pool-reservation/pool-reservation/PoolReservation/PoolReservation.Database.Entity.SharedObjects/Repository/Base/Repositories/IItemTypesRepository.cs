using PoolReservation.Database.Entity.Model.ItemType;
using PoolReservation.Database.Entity.Model.ItemType.Incoming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.SharedObjects.Repository.Base.Repositories
{
    public interface IItemTypesRepository : IRepository
    {

        bool CanUserAddItemTypes(string userId);
        bool CanUserEditItemTypes(string userId, int itemTypeId);
        bool CanUserViewItemType(string userId, int itemTypeId);
        bool CanUserViewItemTypes(string userId);
        bool CanUserDeleteItemTypes(string userId, int itemTypeId);

        ItemTypes CreateItemType(string userId, CreateItemType addItemType);
        ItemTypes EditItemType(string userId, IncomingEditItemType addItemType);
        void DeleteItemType(string userId, int itemtypeId);

        ItemTypes GetItemTypesById(string userId, int id);

        ItemTypes GetItemTypeByVenueItemId(string userId, int itemId);

        IEnumerable<ItemTypes> GetItemTypesByVenueId(string userId, int venueId);

        IEnumerable<ItemTypes> GetItemTypesByVenueTypeId(string userId, int venueTypeId);

        ItemTypes SetItemTypePrice(string userId, int itemId, decimal price);
    }
}
