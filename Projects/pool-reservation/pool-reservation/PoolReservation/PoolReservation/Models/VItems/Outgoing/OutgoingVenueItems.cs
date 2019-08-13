using PoolReservation.Database.Entity;
using PoolReservation.Models.Icon.Outgoing;
using PoolReservation.Models.VenueItemType.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.VenueItem.Outgoing
{
    /// <summary>
    /// OutgoingVenueItems
    /// </summary>
    public class OutgoingVenueItems

    {
        /// <summary>
        /// The id for the venue item.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name for the venue item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The price for 1 of this object.
        /// </summary>
        public decimal Price { get; set; }

        public int NormalQuantity { get; set; }

        public OutgoingVenueItemType Type { get; set; }

        public OutgoingIcon Icon { get; set; }

        public int? IconId { get; set; }

        public bool IsHidden { get; set; }


        public static OutgoingVenueItems Parse(VenueItems x)
        {
            if(x == null)
            {
                return null;
            }

            return new OutgoingVenueItems
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Type = OutgoingVenueItemType.Parse(x.ItemTypes),
                IconId = x.IconId,
                Icon = OutgoingIcon.Parse(x.Icons),
                IsHidden = x.IsHidden
            };
        }

        public static OutgoingVenueItems Parse(VenueItemWithQuantity x)
        {
            if (x == null)
            {
                return null;
            }

            return new OutgoingVenueItems
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                NormalQuantity = x.Quantity,
                IconId = x.IconId
            };
        }
    }
}