using PoolReservation.Models.Venue.Outgoing;
using PoolReservation.Models.VenueType.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Venues.Outgoing
{
    public class OutgoingMinimalVenue
    {
        /// <summary>
        /// The id for the outgoing venue.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Name for the outgoing venue.
        /// </summary>
        public string Name { get; set; }

        public int? StartupMessageId { get; set; }

        public OutgoingVenueType Type { get; set; }

        public bool IsHidden { get; set; }

        public static OutgoingMinimalVenue Parse(PoolReservation.Database.Entity.Venues x)
        {
            if (x == null)
            {
                return null;
            }

            return new OutgoingMinimalVenue
            {
                Id = x.Id,
                Name = x.Name,
                Type = OutgoingVenueType.Parse(x.VenueTypes),
                StartupMessageId = x.StartupMessage,
                IsHidden = x.IsHidden
            };
        }
    }
}