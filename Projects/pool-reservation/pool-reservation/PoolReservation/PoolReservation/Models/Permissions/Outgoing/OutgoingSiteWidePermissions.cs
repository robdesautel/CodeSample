using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PoolReservation.Database.Entity;

namespace PoolReservation.Models.Permissions.Outgoing
{
    /// <summary>
    /// Site wide permissions
    /// </summary>
    public class OutgoingSiteWidePermissions
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
        public OutgoingPermissions Price { get; set; }
        /// <summary>
        /// Permissions for booking other peoples reservations
        /// </summary>
        public OutgoingPermissions OtherReservations { get; set; }
        /// <summary>
        /// Permissions for icons
        /// </summary>
        public OutgoingPermissions Icon { get; set; }
        /// <summary>
        /// Permissions for booking personal reservations
        /// </summary>
        public OutgoingPermissions PersonalReservation { get; set; }

        public static OutgoingSiteWidePermissions Parse(SitePermissions x)
        {
            if(x == null)
            {
                return null;
            }

            return new OutgoingSiteWidePermissions
            {
                Id = x.Id,
                Name = x.Name,
                Hotel = OutgoingPermissions.Parse(x.HotelPermissions),
                Price = OutgoingPermissions.Parse(x.PricePermissions),
                OtherReservations = OutgoingPermissions.Parse(x.OtherReservationsPermissions),
                Icon = OutgoingPermissions.Parse(x.IconPermissions),
                PersonalReservation = OutgoingPermissions.Parse(x.PersonalReservationsPermissions)
            };
        }
    }
}