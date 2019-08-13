using PoolReservation.Database.Entity;
using PoolReservation.Models.Miscellaneous.Outgoing;
using PoolReservation.Models.VenueType.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Venue.Outgoing
{
    /// <summary>
    /// OutgoingVenue
    /// </summary>
    public class OutgoingVenue
    {
        /// <summary>
        /// The id for the outgoing venue.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Name for the outgoing venue.
        /// </summary>
        public string Name { get; set; }

        public OutgoingMiscellaneousHtml StartupMessage { get; set; }

        public OutgoingVenueType Type { get; set; }
        public bool IsHidden { get; set; }

        public static OutgoingVenue Parse(PoolReservation.Database.Entity.Venues x)
        {
            if(x == null)
            {
                return null;
            }

            return new OutgoingVenue
            {
                Id = x.Id,
                Name = x.Name,
                Type = OutgoingVenueType.Parse(x.VenueTypes),
                StartupMessage = OutgoingMiscellaneousHtml.Parse(x.MiscellaneousHtmlTable),
                IsHidden = x.IsHidden
            };
        }
    }
}