using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Miscellaneous.Incoming
{
    public class IncomingNewMessage
    {
        public string PageName { get; set; }

        public string PageData { get; set; }

        public DateTime DateCreated { get; set; }
    }
}