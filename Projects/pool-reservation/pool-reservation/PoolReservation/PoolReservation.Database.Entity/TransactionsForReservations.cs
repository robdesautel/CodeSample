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
    
    public partial class TransactionsForReservations
    {
        public int ReservationGroupId { get; set; }
        public System.Guid TransactionId { get; set; }
        public System.DateTime DateLinked { get; set; }
    
        public virtual ReservationTransaction ReservationTransaction { get; set; }
        public virtual ReservationGroup ReservationGroup { get; set; }
    }
}
