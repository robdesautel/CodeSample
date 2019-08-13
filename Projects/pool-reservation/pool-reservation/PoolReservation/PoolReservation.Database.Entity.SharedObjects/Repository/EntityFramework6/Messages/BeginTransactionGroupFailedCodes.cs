using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6.Messages
{
    public enum BeginTransactionGroupFailedCodes
    {
        HOTEL_NOT_FOUND = 1,
        NO_ITEMS_IN_RESERVATION = 2,
        USER_NOT_FOUND = 3, 
        UNKNOWN_INVALID_GROUP = 4 ,
        NOT_AUTHORIZED = 5
    }
}
