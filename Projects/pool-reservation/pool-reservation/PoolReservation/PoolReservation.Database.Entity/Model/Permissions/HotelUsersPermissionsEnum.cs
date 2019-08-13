using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.Model.Permissions
{
    public enum HotelUsersPermissionsEnum
    {
        FULL_ADMIN = 1,
        MANAGER = 3,
        LIMITED_MANAGER = 4,
        REGULAR_APP_USER = 5,
        BANNED = 6
    }
}
