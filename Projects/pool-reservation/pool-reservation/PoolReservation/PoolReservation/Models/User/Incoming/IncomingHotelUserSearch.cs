using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.User.Incoming
{
    public class IncomingHotelUserSearch
    {
        public int HotelId { get; set; }
        public string Query { get; set; }
        public int StartsWith { get; set; }
        public int NumberToGet { get; set; }
    }
}