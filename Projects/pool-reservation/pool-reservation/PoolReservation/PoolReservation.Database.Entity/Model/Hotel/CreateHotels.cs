using PoolReservation.SharedObjects.Model.File.Incoming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.Model.Hotel
{
    public class CreateHotels
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public decimal TaxRate { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public IncomingBase64File Image{ get; set; }

        public bool IsHidden { get; set; }
    }
}
