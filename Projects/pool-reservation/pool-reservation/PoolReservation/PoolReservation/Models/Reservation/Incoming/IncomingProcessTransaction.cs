using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Reservation.Incoming
{
    public class IncomingProcessTransaction
    {
        public int ReservationId { get; set; }

        public string StripeTokenId { get; set; }

    }
}