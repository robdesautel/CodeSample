using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.Model.Reservations
{
    public enum ReservationSearchTypeEnum
    {
        UserId = 0,
        UserEmail = 1,
        ReservationId = 2,
        HotelId = 3,

    }
}
