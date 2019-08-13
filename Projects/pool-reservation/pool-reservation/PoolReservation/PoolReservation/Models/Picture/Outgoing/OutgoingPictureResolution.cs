using PoolReservation.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Picture.Outgoing
{
    public class OutgoingPictureResolution
    {
        public string BlobUrl { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Size { get; set; }

        public static OutgoingPictureResolution Parse(PictureResolutions x)
        {
            if(x == null)
            {
                return null;
            }

            return new OutgoingPictureResolution
            {
                Height = x.Height,
                Width = x.Width,
                Size = x.Size,
                BlobUrl = x.PictureUrls?.UrlPrefix + "/" + x.FileName
            };
        }

    }
}