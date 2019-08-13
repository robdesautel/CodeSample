using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Venues.Incoming
{
    /// <summary>
    /// Add Venues
    /// </summary>
    public class IncomingVenues
    {

        /// <summary>
        /// Add Venue Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Add Venue Id.
        /// </summary>
        public int Id { get; set; }

        public bool IsHidden { get; set; }
    }
}