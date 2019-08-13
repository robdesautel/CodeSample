using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Permissions.Incoming
{
    public class IncomingHotelPermissionsChange
    {
        public int HotelId { get; set; }
        public string UserIdToChange { get; set; }
        public int PermissionsId { get; set; }

    }
}