using PoolReservation.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Reservation.Outgoing
{
    public class OutgoingReservationGroup
    {
        public int HotelId { get; private set; }
        public int Id { get; private set; }
        public OutgoingReservationGroupStatus Status { get; set; }

        public DateTime? StatusDate { get; set; }
        public Guid? StatusGuid { get; set; }

        public ICollection<OutgoingReservationItem> ReserveItems { get; set; }

        public string UserId { get; private set; }

        public static OutgoingReservationGroup Parse(ReservationGroup x)
        {
            if (x == null)
            {
                return null;
            }

            return new OutgoingReservationGroup
            {
                Id = x.Id,
                Status = OutgoingReservationGroupStatus.Parse(x.ReservationGroupStatus),
                UserId = x.UserId,
                HotelId = x.HotelId,
                StatusDate = x.StatusDate,
                StatusGuid = x.StatusGuid,
                ReserveItems = x.ReserveItems?.Select(y => OutgoingReservationItem.Parse(y))?.ToList()
            };
        }
    }
}
