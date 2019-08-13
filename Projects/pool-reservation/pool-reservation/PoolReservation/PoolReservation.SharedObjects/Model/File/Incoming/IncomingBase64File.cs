using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.SharedObjects.Model.File.Incoming
{
    public class IncomingBase64File
    {
        public string FileName { get; set; }
        public int Size { get; set; }
        public string Data { get; set; }

    }
}