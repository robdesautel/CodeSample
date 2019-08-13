using PoolReservation.Database.Entity.Model.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Reservation.Incoming
{
    public class IncomingSearchAllReservation
    {
        public string Query { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public ReservationSearchTypeEnum? SearchType { get; set; }
    }
}