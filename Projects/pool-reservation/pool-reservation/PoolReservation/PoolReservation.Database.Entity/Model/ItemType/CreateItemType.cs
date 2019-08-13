using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.Model.ItemType
{
    public class CreateItemType
    {
        public int ItemTypeId { get; set; }

        public string Name { get; set; }

        public int VenueTypeId { get; set; }

    }
}
