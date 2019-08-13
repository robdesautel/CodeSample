using PoolReservation.Database.Entity.Model.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6.Messages
{
    public class BeginTransactionItemSummary
    {
        public Status<BeginTransactionItemFailedCodes> Status { get; set; } = new Status<BeginTransactionItemFailedCodes>();

        public BeginReservationItem OriginalObject { get; set; }

        public int? ReservationItemId { get; set; }

        public decimal? FinalPrice { get; set; }
    }
}
