using PoolReservation.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.User.Outgoing
{
    public class OutgoingMinimalUser
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


        public static OutgoingMinimalUser Parse(AspNetUsers x)
        {
            if (x == null)
            {
                return null;
            }

            return new OutgoingMinimalUser
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName
            };
        }
    }
}