using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.Model.Reservations
{
    public enum ReservationGroupStatusEnum
    {
        PENDING    = 1,
        PROCESSING = 2,
        CANCELLED  = 3,
        FAILED     = 4,
        COMPLETED  = 5,
        REFUNDED   = 6
    }
}
