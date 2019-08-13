using PoolReservation.Database.Entity.Model.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Reservation.Incoming
{
    /// <summary>
    /// IncomingBeginReservation
    /// </summary>
    public class IncomingBeginReservation
    {
        public string UserId { get; set; }

        public int HotelId { get; set; }

        public ICollection<IncomingReservationItem> Items { get; set; }

        public BeginReservation CreateDBModel()
        {
            return new BeginReservation
            {
                UserId = UserId,
                Items = Items?.Select(x => x.CreateDBModel()).ToList(),
                HotelId = HotelId
            };
        }
    }
}