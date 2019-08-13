using PoolReservation.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Permissions.Outgoing
{
    /// <summary>
    /// OutgoingPermissions
    /// </summary>
    public class OutgoingPermissions
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool View { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }


        public static OutgoingPermissions Parse(PoolReservation.Database.Entity.Permissions x)
        {
            if(x == null)
            {
                return null;
            }

            return new OutgoingPermissions{
                Id = x.Id,
                Name = x.Name,
                View = x.View,
                Add = x.Add,
                Delete = x.Delete,
                Edit = x.Edit
            };
        }
    }
}