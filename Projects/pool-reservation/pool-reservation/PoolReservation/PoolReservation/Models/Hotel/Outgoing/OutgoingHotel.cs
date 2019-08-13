using PoolReservation.Database.Entity;
using PoolReservation.Models.Picture.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Hotel.Outgoing
{
    /// <summary>
    /// Outgoing Hotel Object
    /// </summary>
    public class OutgoingHotel
    {
        /// <summary>
        /// The identifier.
        /// </summary>
        /// <value>
        /// 
        /// </value>
        public int Id { get; set; }
        /// <summary>
        /// The name.
        /// </summary>
        public string Name { get; set; }

        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public decimal TaxRate { get; set; }
        public OutgoingMinimalPicture Picture { get; set; }

        public bool IsHidden { get; set; }

        public static OutgoingHotel Parse(Hotels x)
        {
            if(x == null)
            {
                return null;
            }

            return new OutgoingHotel
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                TaxRate = x.TaxRate,
                Picture = OutgoingMinimalPicture.Parse(x.Pictures),
                IsHidden = x.IsHidden
            };
        }
    }
}