using PoolReservation.Database.Entity.Model.MiscellaneousHtmlTable;
using PoolReservation.Database.Entity.SharedObjects.Repository.Base.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PoolReservation.Database.Entity.Model.MiscellaneousHtmlTable.Incoming;

namespace PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6.Repositories
{
    class MiscellaneousRepository : Repository, IMiscellaneousRepository
    {

        public MiscellaneousRepository(PoolReservationEntities context, UnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }
        public MiscellaneousHtmlTable GetMessageByVenueId(int venueId)
        {
            return this.dbContext.Venues.Include(x => x.MiscellaneousHtmlTable).FirstOrDefault(x => x.Id == venueId)?.MiscellaneousHtmlTable;
        }
        public MiscellaneousHtmlTable AddMessage(CreateMessage addMessage)
        {
            var newMessage = new MiscellaneousHtmlTable
            {
                PageName = addMessage.PageName,
                PageData = addMessage.PageData,
                DateCreated = DateTime.UtcNow
            };

            var venue = this.dbContext.Venues.FirstOrDefault(x => x.Id == addMessage.VenueId);

            if (venue == null)
            {
                throw new Exception();
            }

            venue.MiscellaneousHtmlTable = newMessage;

            return newMessage;
        }

        public MiscellaneousHtmlTable GetLatestPrivacyPolicy()
        {
            return this.GetLatestPrivacyPolicyForDate(DateTime.Now);
        }

        public MiscellaneousHtmlTable GetLatestPrivacyPolicyForDate(DateTime theDate)
        {
            return this.dbContext.PrivacyPolicy.Where(x => x.DateEffective <= theDate).OrderByDescending(x => x.DateEffective)?.Select(x => x.MiscellaneousHtmlTable).FirstOrDefault();
        }

        public MiscellaneousHtmlTable GetLatestTermsAndConditions()
        {
            return this.GetLatestTermsAndConditionsForDate(DateTime.Now);
        }

        public MiscellaneousHtmlTable GetLatestTermsAndConditionsForDate(DateTime theDate)
        {
            return this.dbContext.TermsAndConditions.Where(x => x.DateEffective <= theDate).OrderByDescending(x => x.DateEffective)?.Select(x => x.MiscellaneousHtmlTable).FirstOrDefault();
        }

        public IEnumerable<InboxMessages> GetMessagesForNumberOfDays(string userId, int numberOfDays)
        {
            var earliestDate = DateTime.UtcNow - TimeSpan.FromDays(numberOfDays);

            return this.dbContext.InboxMessages.Include(x => x.UserSentBy).Include(x => x.MiscellaneousHtmlTable).Where(x => x.UserForId == userId && x.DateSent >= earliestDate);
        }

        public MiscellaneousHtmlTable GetMiscellaneousMessage(int id)
        {
            return this.dbContext.MiscellaneousHtmlTable.FirstOrDefault(x => x.Id == id);
        }

        public MiscellaneousHtmlTable UpdateSpecialMessage(IncomingSpecialMessage updateSpecialMessage)
        {
            var editSpecialMessage = dbContext.MiscellaneousHtmlTable.FirstOrDefault(x => x.Id == updateSpecialMessage.VenueId);

            if (editSpecialMessage == null)
            {
                throw new Exception("No message");
            }

            editSpecialMessage.PageName = updateSpecialMessage.PageName;
            editSpecialMessage.PageData = updateSpecialMessage.PageData;
            //DateUpdated = DateTime.UtcNow; //add DateUpdated to the MiscellaneousHtmlTable

            return editSpecialMessage;
        }
    }
}
