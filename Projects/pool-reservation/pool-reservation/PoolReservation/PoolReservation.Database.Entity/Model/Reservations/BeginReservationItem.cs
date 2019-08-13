using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.Model.Reservations
{
    public class BeginReservationItem
    {
        public DateTime Date { get; set; }


        public int ItemId { get; set; }

        public int Quantity { get; set; }
    }
}
