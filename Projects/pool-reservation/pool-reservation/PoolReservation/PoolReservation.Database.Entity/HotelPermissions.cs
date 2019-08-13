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
    
    public partial class HotelPermissions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HotelPermissions()
        {
            this.HotelUsers = new HashSet<HotelUsers>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int Items { get; set; }
        public int PersonalReservations { get; set; }
        public int OtherReservations { get; set; }
        public int Hotel { get; set; }
        public int Users { get; set; }
    
        public virtual Permissions HotelPermission { get; set; }
        public virtual Permissions ItemPermissions { get; set; }
        public virtual Permissions OtherReservationsPermissions { get; set; }
        public virtual Permissions PersonalReservationPermissions { get; set; }
        public virtual Permissions UserPermissions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HotelUsers> HotelUsers { get; set; }
    }
}