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
    
    public partial class ReservationGroup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReservationGroup()
        {
            this.ReserveItems = new HashSet<ReserveItems>();
            this.TransactionsForReservations = new HashSet<TransactionsForReservations>();
        }
    
        public int Id { get; set; }
        public string UserId { get; set; }
        public int HotelId { get; set; }
        public System.DateTime DateCreated { get; set; }
        public int StatusId { get; set; }
        public Nullable<System.DateTime> StatusDate { get; set; }
        public Nullable<System.Guid> StatusGuid { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual Hotels Hotels { get; set; }
        public virtual ReservationGroupStatus ReservationGroupStatus { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReserveItems> ReserveItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransactionsForReservations> TransactionsForReservations { get; set; }
    }
}