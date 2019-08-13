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
    
    public partial class Venues
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Venues()
        {
            this.Calendar = new HashSet<Calendar>();
            this.VenueItems = new HashSet<VenueItems>();
        }
    
        public int Id { get; set; }
        public int HotelId { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> StartupMessage { get; set; }
        public bool IsHidden { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Calendar> Calendar { get; set; }
        public virtual Hotels Hotels { get; set; }
        public virtual MiscellaneousHtmlTable MiscellaneousHtmlTable { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VenueItems> VenueItems { get; set; }
        public virtual VenueTypes VenueTypes { get; set; }
    }
}