using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6.Messages
{
    public enum BeginTransactionItemFailedCodes
    {
        NULL_ITEM = 1,
        ITEM_NOT_FOUND = 2,
        UNKNOWN_ITEM_EXCEPTION = 3,
        QUANTITY_TOO_SMALL = 4,
        UNKNOWN_INVALID_DAY = 6,
        NULL_DAY = 7,
        INVALID_SAME_DAY = 8
    }
}
