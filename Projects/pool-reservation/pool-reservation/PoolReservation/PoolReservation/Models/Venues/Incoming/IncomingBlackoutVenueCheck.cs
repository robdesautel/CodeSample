using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Venues.Incoming
{
    public class IncomingBlackoutVenueCheck
    {

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int VenueId { get; set; }
    }
}