using PoolReservation.Database.Entity.Model.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Reservation.Incoming
{
    /// <summary>
    /// IncomingReservationItem
    /// </summary>
    public class IncomingReservationItem
    {
        public int ItemId { get; set; }

        public int Quantity { get; set; }

        public DateTime Date { get; set; }

        public BeginReservationItem CreateDBModel()
        {
            return new BeginReservationItem
            {
                ItemId = ItemId,
                Quantity = Quantity,
                Date = Date
            };
        }
    }
}