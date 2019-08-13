using PoolReservation.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.VenueItemType.Outgoing
{
    /// <summary>
    /// OutgoingVenueItemType
    /// </summary>
    public class OutgoingVenueItemType
    {
        /// <summary>
        /// Gets the Venue Item Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets the Venu Item Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the Venue Item Price
        /// </summary>
        public decimal Price { get; set; }

        public static OutgoingVenueItemType Parse(ItemTypes x)
        {
            if (x == null)
            {
                return null;
            }

            return new OutgoingVenueItemType
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price
            };
        }
    }
}