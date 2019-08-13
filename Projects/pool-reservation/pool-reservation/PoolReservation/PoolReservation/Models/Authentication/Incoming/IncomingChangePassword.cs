using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Authentication.Incoming
{
    public class IncomingChangePassword
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }

    }
}