using PoolReservation.Database.Entity;
using PoolReservation.Models.Permissions.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.User.Outgoing
{
    public class OutgoingHotelUser
    {
        public int HotelId { get; set; }

        public OutgoingMinimalUser User { get; set; }

        public OutgoingHotelPermissions Permissions { get; set; }
        public static OutgoingHotelUser Parse(HotelUsers x)
        {
            if(x == null)
            {
                return null;
            }

            return new OutgoingHotelUser
            {
                HotelId = x.HotelId,
                User = OutgoingMinimalUser.Parse(x.AspNetUsers),
                Permissions = OutgoingHotelPermissions.Parse(x.HotelPermissions)
            };
        }
    }
}