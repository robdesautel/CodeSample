using PoolReservation.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Hotel.Outgoing
{
    public class OutgoingDetailedHotel
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
        public decimal TaxRate { get; set; }

        public bool IsHidden { get; set; }


        public static OutgoingDetailedHotel Parse(Hotels x)
        {
            if (x == null)
            {
                return null;
            }

            return new OutgoingDetailedHotel
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                TaxRate = x.TaxRate,
                IsHidden = x.IsHidden

            };
        }
    }
}