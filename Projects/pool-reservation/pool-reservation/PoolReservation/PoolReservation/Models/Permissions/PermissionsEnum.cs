using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Permissions
{
    public enum PermissionsEnum
    {
        FULL_ACCESS = 1,
        VIEW_ONLY = 4,
        BANNED = 6
    }
}