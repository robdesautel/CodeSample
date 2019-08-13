using PoolReservation.Database.Entity;
using PoolReservation.Models.Picture.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Icon.Outgoing
{
    public class OutgoingIcon
    {
        public int Id { get; set; }

        public OutgoingMinimalPicture Picture { get; set; }

        public static OutgoingIcon Parse(Icons x)
        {
            if (x == null)
            {
                return null;
            }

            return new OutgoingIcon
            {
                Id = x.Id,
                Picture = OutgoingMinimalPicture.Parse(x.Pictures)
            };
        }
    }
}