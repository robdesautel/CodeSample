using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.Model.Item.Incoming
{
    public class IncomingEditItem
    {
        public int Id { get; set; }

        public string Name { get; set; }


        public decimal Price { get; set; }

        public int IconId { get; set; }

        public bool IsHidden { get; set; }


    }
}
