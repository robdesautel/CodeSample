using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.Model.Reservations
{
    public class BeginReservation
    {
        public string UserId { get; set; }

        public int HotelId { get; set; }
        public ICollection<BeginReservationItem> Items { get; set; }
    }
}
