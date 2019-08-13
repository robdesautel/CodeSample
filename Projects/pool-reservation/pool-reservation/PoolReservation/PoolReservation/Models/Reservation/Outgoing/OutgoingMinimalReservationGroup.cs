using PoolReservation.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Reservation.Outgoing
{
    public class OutgoingMinimalReservationGroup
    {
        public int HotelId { get; private set; }
        public int Id { get; private set; }
        public string UserId { get; private set; }

        public static OutgoingMinimalReservationGroup Parse(ReservationGroup x)
        {
           if(x == null)
            {
                return null;
            }

            return new OutgoingMinimalReservationGroup
            {
                Id = x.Id,
                UserId = x.UserId,
                HotelId = x.HotelId
            };
        }
    }
}