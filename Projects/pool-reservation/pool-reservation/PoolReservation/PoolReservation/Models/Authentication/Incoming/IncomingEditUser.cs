using PoolReservation.SharedObjects.Model.File.Incoming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Authentication.Incoming
{
    public class IncomingEditUser
    {
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IncomingBase64File Image { get; set; }
    }
}