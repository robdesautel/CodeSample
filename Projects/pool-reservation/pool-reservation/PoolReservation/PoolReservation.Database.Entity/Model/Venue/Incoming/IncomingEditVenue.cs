using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.Model.Venue.Incoming
{
    public class IncomingEditVenue
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public bool IsHidden { get; set; }
    }
}
