using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Location.Incoming
{
    public class IncomingBoundingBoxLocation
    {
        /// <summary>
        /// The latitude.
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// The longitude.
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// The distance in kilometers.
        /// </summary>
        public double Distance { get; set; }
    }
}