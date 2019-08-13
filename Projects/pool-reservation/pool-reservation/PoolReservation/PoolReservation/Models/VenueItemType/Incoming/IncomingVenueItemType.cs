using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.VenueItemType.Incoming
{
    /// <summary>
    /// Incoming Venue Item Type
    /// </summary>
    public class IncomingVenueItemType
    {
        /// <summary>
        /// Venue Item Type Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Venue Item Type Price
        /// </summary>
        public decimal price { get; set; }
    }
}