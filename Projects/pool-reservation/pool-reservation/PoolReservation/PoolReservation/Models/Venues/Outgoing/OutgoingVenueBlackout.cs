using PoolReservation.Database.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Venues.Outgoing
{
    /// <summary>
    /// OutgoingVenueBlackout
    /// </summary>
    public class OutgoingVenueBlackout
    {
        public int Id { get; set; }

        /// <summary>
        /// The venue id
        /// </summary>
        public int VenueId { get; set; }

        /// <summary>
        /// The start date of the blackout
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The end date of the blackout
        /// </summary>
        public DateTime EndDate { get; set; }


        public static OutgoingVenueBlackout Parse(Blackout x)
        {
            if(x == null)
            {
                return null;
            }

            return new OutgoingVenueBlackout
            { 
                Id = x.Id,
                VenueId = x.VenueId,
                StartDate = x.StartDate,
                EndDate = x.EndDate
            };
        }
    }
}