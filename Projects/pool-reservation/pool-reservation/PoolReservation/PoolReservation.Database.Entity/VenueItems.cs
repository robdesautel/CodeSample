//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PoolReservation.Database.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class VenueItems
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VenueItems()
        {
            this.VenueItemQuantity = new HashSet<VenueItemQuantity>();
            this.ReserveItems = new HashSet<ReserveItems>();
        }
    
        public int Id { get; set; }
        public int VenueId { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; }
        public int IconId { get; set; }
        public bool IsHidden { get; set; }
    
        public virtual ItemTypes ItemTypes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VenueItemQuantity> VenueItemQuantity { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReserveItems> ReserveItems { get; set; }
        public virtual Icons Icons { get; set; }
        public virtual Venues Venues { get; set; }
    }
}