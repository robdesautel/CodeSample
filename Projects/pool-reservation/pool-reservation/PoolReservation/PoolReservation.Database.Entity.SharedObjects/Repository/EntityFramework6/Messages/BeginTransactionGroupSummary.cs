using PoolReservation.Database.Entity.Model.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6.Messages
{
    public class BeginTransactionGroupSummary
    {
        public Status<BeginTransactionGroupFailedCodes> Status { get; set; } = new Status<BeginTransactionGroupFailedCodes>();

        public int? ReservationGroupId { get; set; }

        public BeginReservation OriginalObject { get; set; }

        public ICollection<BeginTransactionItemSummary> ItemSummaries { get; set; } = new List<BeginTransactionItemSummary>();

        public decimal? FinalPrice { get; set; }




    }
}
