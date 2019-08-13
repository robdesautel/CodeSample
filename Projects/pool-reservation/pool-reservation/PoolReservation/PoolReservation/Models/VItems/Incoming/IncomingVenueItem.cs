using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.VItems.Incoming
{
    /// <summary>
    /// Add Venue Items
    /// </summary>
    public class IncomingVenueItem
    {
        /// <summary>
        /// Add name of the Venue Item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Add the Id of the Venue Item
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Add the Normal Quantity of the Venue Item
        /// </summary>
        public int NormalQuantity { get; set; }

        public bool IsHidden { get; set; }
    }
}