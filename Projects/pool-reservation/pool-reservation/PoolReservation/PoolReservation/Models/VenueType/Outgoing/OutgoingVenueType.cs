using PoolReservation.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.VenueType.Outgoing
{
    /// <summary>
    /// OutgoingVenueType
    /// </summary>
    public class OutgoingVenueType
    {
        /// <summary>
        /// Gets Id for the Venue Type
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets the Name of the Venue Type
        /// </summary>
        public string Name { get; set; }

        public static OutgoingVenueType Parse(VenueTypes x)
        {
            if (x == null)
            {
                return null;
            }

            return new OutgoingVenueType
            {
                Id = x.Id,
                Name = x.Name
            };
        }
    }
}