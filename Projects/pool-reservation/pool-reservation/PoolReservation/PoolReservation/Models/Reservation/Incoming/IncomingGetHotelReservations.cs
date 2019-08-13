using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Reservation.Incoming
{
    public class IncomingGetHotelReservations
    {
        public int HotelId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


    }
}