using PoolReservation.SharedObjects.Model.File.Incoming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.SharedObjects.Model.Authentication.Incoming
{
    public class IncomingRegister
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public IncomingBase64File Image { get; set; }
    }
}
