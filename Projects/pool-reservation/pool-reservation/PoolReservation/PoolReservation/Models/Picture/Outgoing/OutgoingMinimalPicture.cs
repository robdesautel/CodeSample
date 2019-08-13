using PoolReservation.Database.Entity;
using PoolReservation.SharedObjects.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Picture.Outgoing
{
    public class OutgoingMinimalPicture
    {
        public Guid Id { get; set; }
        public ICollection<OutgoingPictureResolution> Resolutions { get; set; }

        public DateTime DateUploaded { get; set; }

        public static OutgoingMinimalPicture Parse(Pictures x)
        {
            if(x == null)
            {
                return null;
            }

            if(x.IsDeleted == true)
            {
                return null;
            }

            return new OutgoingMinimalPicture
            {
                Id = x.Id,
                Resolutions = x.PictureResolutions?.Select(y => OutgoingPictureResolution.Parse(y))?.RemoveNulls()?.ToList(),
                DateUploaded = x.DateUploaded
            };
        }
    }
}