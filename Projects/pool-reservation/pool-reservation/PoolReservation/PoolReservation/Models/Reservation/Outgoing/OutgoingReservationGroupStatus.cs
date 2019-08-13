using PoolReservation.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Reservation.Outgoing
{
    public class OutgoingReservationGroupStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static OutgoingReservationGroupStatus Parse(ReservationGroupStatus x)
        {
            if (x == null)
            {
                return null;
            }

            return new OutgoingReservationGroupStatus
            {
                Id = x.Id,
                Name = x.Name
            };
        }
    }
}