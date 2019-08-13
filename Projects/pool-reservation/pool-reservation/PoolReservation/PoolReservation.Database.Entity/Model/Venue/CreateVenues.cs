using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.Model.Venue
{
    public class CreateVenues
    {
        public string Name { get; set; }

        public int HotelId { get; set; }

        public int VenueTypeId { get; set; }

        public bool IsHidden { get; set; }


    }
}
