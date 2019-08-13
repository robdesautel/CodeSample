using PoolReservation.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.User.Outgoing
{
    public class OutgoingIdEmailUser
    {
        /// <summary>
        /// The id for the user.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The email of the user.
        /// </summary>
        public string Email { get; set; }




        public static OutgoingIdEmailUser Parse(AspNetUsers x)
        {
            if (x == null)
            {
                return null;
            }

            return new OutgoingIdEmailUser
            {
                Id = x.Id,
                Email = x.Email
            };
        }
    }
}