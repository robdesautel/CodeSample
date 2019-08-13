using PoolReservation.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Miscellaneous.Outgoing
{
    public class OutgoingMiscellaneousHtml
    {
        public int Id { get; set; }
        public string PageName { get; set; }
        public string PageData { get; set; }

        public static OutgoingMiscellaneousHtml Parse(MiscellaneousHtmlTable x)
        {
            if(x == null)
            {
                return null;
            }

            return new OutgoingMiscellaneousHtml
            {
                Id = x.Id,
                PageName = x.PageName,
                PageData = x.PageData
            };
        }

    }
}