using PoolReservation.Database.Entity.Model.MiscellaneousHtmlTable;
using PoolReservation.Database.Entity.Model.MiscellaneousHtmlTable.Incoming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.SharedObjects.Repository.Base.Repositories
{
    public interface IMiscellaneousRepository : IRepository
    {

        MiscellaneousHtmlTable AddMessage(CreateMessage addMessage);
        MiscellaneousHtmlTable GetLatestTermsAndConditions();
        MiscellaneousHtmlTable GetLatestTermsAndConditionsForDate(DateTime theDate);

        MiscellaneousHtmlTable GetLatestPrivacyPolicy();
        MiscellaneousHtmlTable GetLatestPrivacyPolicyForDate(DateTime theDate);

        IEnumerable<InboxMessages> GetMessagesForNumberOfDays(string userId, int numberOfDays);

        MiscellaneousHtmlTable GetMiscellaneousMessage(int id);

        MiscellaneousHtmlTable GetMessageByVenueId(int venueId);

        MiscellaneousHtmlTable UpdateSpecialMessage(IncomingSpecialMessage updateSpecialMessage);
    }
}
