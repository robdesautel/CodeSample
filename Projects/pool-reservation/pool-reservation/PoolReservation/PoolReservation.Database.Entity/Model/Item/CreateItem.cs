using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.Model.Item
{
    public class CreateItem
    {

        public int ItemTypeId { get; set; }

        public int VenueId { get; set; }

        public string Name { get; set; }

        public int NormalQuantity { get; set; }

        public decimal Price { get; set; }

        public int IconId { get; set; }

        public bool IsHidden { get; set; }
    }
}
