using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.SharedObjects.Model.Message.Outgoing
{
    public class OutgoingMessage
    {
        public string Message { get; set; }
        public string DetailedMessage { get; set; }
        public string Action { get; set; }
        public string DetailedAction { get; set; }
    }
}
