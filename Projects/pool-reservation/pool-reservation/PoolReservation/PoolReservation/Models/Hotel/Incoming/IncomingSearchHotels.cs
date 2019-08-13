using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Hotel.Incoming
{
    public class IncomingSearchHotels
    {
        /// <summary>
        /// The query that you want to search by.
        /// </summary>
        public string query { get; set; }
        /// <summary>
        /// The number of items to skip.
        /// </summary>
        public int? startingIndex { get; set; }
        /// <summary>
        /// The number of items to get.
        /// </summary>
        public int? numberToGet { get; set; }

        public bool IncludeHidden { get; set; }


    }
}