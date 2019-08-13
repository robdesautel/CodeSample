using PoolReservation.Database.Entity;
using PoolReservation.Models.User.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoolReservation.Models.Miscellaneous.Outgoing
{
    public class OutgoingInboxMessage
    {
        public int Id { get; set; }
        public DateTime DateSent { get; set; }
        public int MiscellaneousHtmlId { get; set; }
        public string Title { get; set; }
        public OutgoingMinimalUser UserSentBy { get; set; }

        public static OutgoingInboxMessage Parse(InboxMessages x)
        {
            if(x == null)
            {
                return null;
            }
            
            return new OutgoingInboxMessage
            {
                Id = x.Id,
                DateSent = x.DateSent,
                MiscellaneousHtmlId = x.MiscellaneousId,
                Title = x.MiscellaneousHtmlTable.PageName,
                UserSentBy = OutgoingMinimalUser.Parse(x.UserSentBy)
            };
        }
    }
}