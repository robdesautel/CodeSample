using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.Model.Reservations
{
    public enum TransactionStatusEnum
    {
        PENDING = 1,
        SUCCESSFUL = 2,
        FAILED = 3,
        CANCELLED = 4
    }
}
