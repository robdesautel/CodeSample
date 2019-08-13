using PoolReservation.Database.Entity;
using PoolReservation.Models.Permissions.Outgoing;
using PoolReservation.Models.Picture.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.User.Outgoing
{
    /// <summary>
    /// OutgoingVenueItems
    /// </summary>
    public class OutgoingPersonalUser
    {
        /// <summary>
        /// The id for the user.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The first name of the user.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the user.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        public string PhoneNumber { get; set; }


        public OutgoingSiteWidePermissions SiteWidePermissions { get; set; }

        public OutgoingMinimalPicture ProfilePicture { get; set; }

        public static OutgoingPersonalUser Parse(AspNetUsers x) {
            if(x == null)
            {
                return null;
            }

            return new OutgoingPersonalUser
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                SiteWidePermissions = OutgoingSiteWidePermissions.Parse(x.SitePermissions),
                ProfilePicture = OutgoingMinimalPicture.Parse(x.ProfilePicture)
            };
        }


    }
}