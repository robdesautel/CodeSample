using PoolReservation.SharedObjects.Model.File.Incoming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Hotel.Incoming
{
    /// <summary>
    /// Add Hotels
    /// </summary>
    public class IncomingHotel
    {
        /// <summary>
        /// Add Hotel Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Add Hotel Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Add the hotels tax rate.
        /// </summary>
        public decimal TaxRate { get; set; }

        public bool IsHidden { get; set; }

    }
}