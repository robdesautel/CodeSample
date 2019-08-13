using Microsoft.AspNet.Identity;
using PoolReservation.Database.Entity.Model.MiscellaneousHtmlTable;
using PoolReservation.Database.Entity.Model.MiscellaneousHtmlTable.Incoming;
using PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6;
using PoolReservation.Infrastructure.Errors;
using PoolReservation.Infrastructure.Http;
using PoolReservation.Models.Miscellaneous.Incoming;
using PoolReservation.Models.Miscellaneous.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace PoolReservation.api
{
    public class MiscellaneousController : ApiController
    {

        [HttpPut]
        [Route("api/Miscellaneous/UpdateSpecialMessage")]
        [ResponseType(typeof(OutgoingMiscellaneousHtml))]
        [Authorize]
        public HttpResponseMessage UpdateSpecialMessage (IncomingSpecialMessage updateSpecialMessage)
        {
            return ErrorFactory.Handle(() =>
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var updatedSpecialMessage = unitOfWork.Miscellaneous.UpdateSpecialMessage(updateSpecialMessage);

                    unitOfWork.Complete();

                    var outgoingMiscellaneousHtml = OutgoingMiscellaneousHtml.Parse(updatedSpecialMessage);

                    return JsonFactory.CreateJsonMessage(outgoingMiscellaneousHtml, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }

        [HttpGet]
        [Route("api/Miscellaneous/GetSpecialMessageByVenueId")]
        [ResponseType(typeof(OutgoingMiscellaneousHtml))]
        [Authorize]
        public HttpResponseMessage GetSpecialMessageByVenueId (int venueId)
        {
            return ErrorFactory.Handle(() =>
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var existingMessage = unitOfWork.Miscellaneous.GetMessageByVenueId(venueId);

                    var outgoingMiscellaneousHtml = OutgoingMiscellaneousHtml.Parse(existingMessage);

                    return JsonFactory.CreateJsonMessage(outgoingMiscellaneousHtml, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }

        [HttpPost]
        [Route("api/Miscellaneous/AddNewMessage")]
        [ResponseType(typeof(OutgoingMiscellaneousHtml))]
        [Authorize]
        public HttpResponseMessage AddNewMessage(CreateMessage addMessage)
        {
            return ErrorFactory.Handle(() =>
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var newMessage = unitOfWork.Miscellaneous.AddMessage(addMessage);

                    unitOfWork.Complete();

                    var outgoingResponse = OutgoingMiscellaneousHtml.Parse(newMessage);

                    return JsonFactory.CreateJsonMessage(outgoingResponse, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }

        [HttpGet]
        [Route("api/Miscellaneous/GetLatestTermsAndConditions")]
        [ResponseType(typeof(OutgoingMiscellaneousHtml))]
        public HttpResponseMessage GetLatestTermsAndConditions()
        {
            return ErrorFactory.Handle(() =>
            {

                using (var unitOfWork = new UnitOfWork())
                {
                    var response = unitOfWork.Miscellaneous.GetLatestTermsAndConditions();

                    var outgoingResponse = OutgoingMiscellaneousHtml.Parse(response);

                    return JsonFactory.CreateJsonMessage(outgoingResponse, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }

        [HttpGet]
        [Route("api/Miscellaneous/GetLatestTermsAndConditions/AsHtml")]
        public HttpResponseMessage GetLatestTermsAndConditionsAsHtml()
        {
            return ErrorFactory.Handle(() =>
            {

                using (var unitOfWork = new UnitOfWork())
                {
                    var response = unitOfWork.Miscellaneous.GetLatestTermsAndConditions();

                    if (response == null)
                    {
                        return JsonFactory.CreateHTMLResponseMessage(@"<html><head><title>Terms and Conditions</title></head><body>Unable to find the latest terms and conditions.</body></html>", HttpStatusCode.NotFound, this.Request);
                    }

                    return JsonFactory.CreateHTMLResponseMessage(response.PageData, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }

        [HttpGet]
        [Route("api/Miscellaneous/")]
        [ResponseType(typeof(OutgoingMiscellaneousHtml))]
        public HttpResponseMessage GetMiscellaneousMessage(int id)
        {
            return ErrorFactory.Handle(() =>
            {

                using (var unitOfWork = new UnitOfWork())
                {
                    var response = unitOfWork.Miscellaneous.GetMiscellaneousMessage(id);

                    var outgoingResponse = OutgoingMiscellaneousHtml.Parse(response);

                    return JsonFactory.CreateJsonMessage(outgoingResponse, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }

        [HttpGet]
        [Route("api/Miscellaneous/AsHtml")]
        [Authorize]
        public HttpResponseMessage GetMiscellaneousMessageAsHtml(int id)
        {
            return ErrorFactory.Handle(() =>
            {

                using (var unitOfWork = new UnitOfWork())
                {
                    var response = unitOfWork.Miscellaneous.GetMiscellaneousMessage(id);

                    if (response == null)
                    {
                        return JsonFactory.CreateHTMLResponseMessage(@"<html><head><title>Unable to find page</title></head><body>Unable to find this page.</body></html>", HttpStatusCode.NotFound, this.Request);
                    }

                    return JsonFactory.CreateHTMLResponseMessage(response.PageData, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }

        [HttpGet]
        [Route("api/Miscellaneous/GetLatestPrivacyPolicy")]
        [ResponseType(typeof(OutgoingMiscellaneousHtml))]
        public HttpResponseMessage GetLatestPrivacyPolicy()
        {
            return ErrorFactory.Handle(() =>
            {

                using (var unitOfWork = new UnitOfWork())
                {
                    var response = unitOfWork.Miscellaneous.GetLatestPrivacyPolicy();

                    var outgoingResponse = OutgoingMiscellaneousHtml.Parse(response);

                    return JsonFactory.CreateJsonMessage(outgoingResponse, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }

        [HttpGet]
        [Route("api/Miscellaneous/GetLatestPrivacyPolicy/AsHtml")]
        public HttpResponseMessage GetLatestPrivacyPolicyAsHtml()
        {
            return ErrorFactory.Handle(() =>
            {

                using (var unitOfWork = new UnitOfWork())
                {
                    var response = unitOfWork.Miscellaneous.GetLatestPrivacyPolicy();

                    if (response == null)
                    {
                        return JsonFactory.CreateHTMLResponseMessage(@"<html><head><title>Privacy Policy</title></head><body>Unable to find the current privacy policy.</body></html>", HttpStatusCode.NotFound, this.Request);
                    }

                    return JsonFactory.CreateHTMLResponseMessage(response.PageData, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }

        /// <summary>
        /// Gets the messages for a user sent in the last numberOfDays.
        /// </summary>
        /// <param name="numberOfDays">The number of days.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Miscellaneous/GetMessagesForUser")]
        [ResponseType(typeof(ICollection<OutgoingInboxMessage>))]
        [Authorize]
        public HttpResponseMessage GetMessagesForUser(int numberOfDays)
        {
            return ErrorFactory.Handle(() =>
            {
                var userId = User.Identity.GetUserId();

                if(userId == null)
                {
                    throw new Exception();
                }

                if(numberOfDays <= 0)
                {
                    throw new Exception();
                }

                using (var unitOfWork = new UnitOfWork())
                {
                    var response = unitOfWork.Miscellaneous.GetMessagesForNumberOfDays(userId, numberOfDays);

                    var outgoingResponse = response?.Select(x => OutgoingInboxMessage.Parse(x))?.ToList();

                    return JsonFactory.CreateJsonMessage(outgoingResponse, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }
    }
}
