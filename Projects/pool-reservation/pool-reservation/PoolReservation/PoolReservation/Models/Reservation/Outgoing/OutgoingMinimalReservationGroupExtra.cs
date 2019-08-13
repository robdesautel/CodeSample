using PoolReservation.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Reservation.Outgoing
{
    public class OutgoingMinimalReservationGroupExtra
    {
        public int HotelId { get; private set; }
        public string HotelName{ get; private set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Id { get; private set; }
        public string UserId { get; private set; }
        public string UsersName { get; private set; }
        public string UsersEmail { get; private set; }
        public int NumberOfItems { get; set; }

        public OutgoingReservationGroupStatus Status { get; set; }

        public static OutgoingMinimalReservationGroupExtra Parse(ReservationGroup x)
        {
            if (x == null)
            {
                return null;
            }

            return new OutgoingMinimalReservationGroupExtra
            {
                Id = x.Id,
                UserId = x.UserId,
                HotelId = x.HotelId,
                HotelName = x.Hotels.Name,
                StartDate = x.ReserveItems?.Where(y => y.IsDeleted == false)?.OrderByDescending(y => y.DateReservedFor)?.FirstOrDefault()?.DateReservedFor,
                EndDate = x.ReserveItems?.Where(y => y.IsDeleted == false)?.OrderBy(y => y.DateReservedFor)?.FirstOrDefault()?.DateReservedFor,
                NumberOfItems = x.ReserveItems?.Where(y => y.IsDeleted == false)?.Count() ?? 0,
                Status = OutgoingReservationGroupStatus.Parse(x.ReservationGroupStatus),
                UsersName = x?.AspNetUsers?.FirstName + " " + x?.AspNetUsers?.LastName,
                UsersEmail = x?.AspNetUsers?.Email
            };
        }
    }
}