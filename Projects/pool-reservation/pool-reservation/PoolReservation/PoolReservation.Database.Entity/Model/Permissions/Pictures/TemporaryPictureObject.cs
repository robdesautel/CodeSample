using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Database.Entity.Model.Pictures
{
    public class TemporaryPictureObject
    {
        public byte[] Data { get; set; }

        public string FileName { get; set; }
        public int FileUrlId { get; set; }

        public int Height { get; set; }
        public int Width { get; set; }
        public int Size { get; set; }


    }
}