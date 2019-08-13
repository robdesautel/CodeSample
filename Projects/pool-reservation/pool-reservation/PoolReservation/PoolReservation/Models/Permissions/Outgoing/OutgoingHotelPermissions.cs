using PoolReservation.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Permissions.Outgoing
{
    public class OutgoingHotelPermissions
    {
        /// <summary>
        /// The identifier of the permissions set.
        /// </summary>
        /// <value>
        /// 
        /// </value>
        public int Id { get; set; }
        /// <summary>
        /// The name of the permissions set.
        /// </summary>
        /// <value>
        /// 
        /// </value>
        public string Name { get; set; }
        /// <summary>
        /// Permissions for hotels
        /// </summary>
        public OutgoingPermissions Hotel { get; set; }
        /// <summary>
        /// Permissions for prices
        /// </summary>
        public OutgoingPermissions Items { get; set; }
        /// <summary>
        /// Permissions for booking other peoples reservations
        /// </summary>
        public OutgoingPermissions Users { get; set; }
        /// <summary>
        /// Permissions for icons
        /// </summary>
        public OutgoingPermissions PersonalReservations { get; set; }
        /// <summary>
        /// Permissions for booking personal reservations
        /// </summary>
        public OutgoingPermissions OtherReservations { get; set; }

        public static OutgoingHotelPermissions Parse(HotelPermissions x)
        {
            if (x == null)
            {
                return null;
            }

            return new OutgoingHotelPermissions
            {
                Id = x.Id,
                Name = x.Name,
                Hotel = OutgoingPermissions.Parse(x.HotelPermission),
                Users = OutgoingPermissions.Parse(x.UserPermissions),
                OtherReservations = OutgoingPermissions.Parse(x.OtherReservationsPermissions),
                Items = OutgoingPermissions.Parse(x.ItemPermissions),
                PersonalReservations = OutgoingPermissions.Parse(x.PersonalReservationPermissions)
            };
        }
    }
}