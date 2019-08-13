using PoolReservation.Database.Entity;
using PoolReservation.Models.VenueItem.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Reservation.Outgoing
{
    public class OutgoingReservationItem
    {
        public DateTime? DeletedDate { get; private set; }
        public int Id { get; private set; }
        public bool IgnoreQuantityRestrictions { get; private set; }
        public bool IsDeleted { get; private set; }
        public int ItemId { get; private set; }
        public decimal PricePreTax { get; private set; }
        public decimal TaxRate { get; private set; }
        public decimal FinalPrice { get; private set; }
        public int Quantity { get; private set; }
        public int ReservationId { get; private set; }
        public OutgoingVenueItems Item { get; set; }

        public static OutgoingReservationItem Parse(ReserveItems x)
        {
            if(x == null)
            {
                return null;
            }

            return new OutgoingReservationItem
            {
                Id = x.Id,
                Quantity = x.Quantity,
                PricePreTax = x.PricePreTax,
                IgnoreQuantityRestrictions = x.IgnoreQuantityRestrictions,
                IsDeleted = x.IsDeleted,
                DeletedDate = x.DeletedDate,
                ItemId = x.ItemId,
                ReservationId = x.ReservationId,
                TaxRate = x.TaxRate,
                FinalPrice = x.FinalPrice,
                Item = OutgoingVenueItems.Parse(x.VenueItems)

            };
        }
    }
}