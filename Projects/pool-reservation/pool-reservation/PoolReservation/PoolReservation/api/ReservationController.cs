using Microsoft.AspNet.Identity;
using PoolReservation.Database.Entity;
using PoolReservation.Database.Entity.Model.Reservations;
using PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6;
using PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6.Messages;
using PoolReservation.Infrastructure.Errors;
using PoolReservation.Infrastructure.Http;
using PoolReservation.Models.Reservation.Incoming;
using PoolReservation.Models.Reservation.Outgoing;
using PoolReservation.SharedObjects.Model.Exceptions.Validation;
using PoolReservation.SharedObjects.Model.Message.Outgoing;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PoolReservation.api
{
    public class ReservationController : ApiController
    {
        /// <summary>
        /// Begins the reservationProcess
        /// </summary>
        /// <param name="reservation">The reservation object</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Reservation/Begin/")]
        [ResponseType(typeof(BeginTransactionGroupSummary))]
        [Authorize]
        public HttpResponseMessage BeginReservationProcess(IncomingBeginReservation reservation)
        {
            return ErrorFactory.Handle(() =>
            {
                var userId = User?.Identity?.GetUserId();

                if (string.IsNullOrWhiteSpace(userId))
                {
                    throw new Exception();
                }

                var dbModel = reservation.CreateDBModel();

                using (var unitOfWork = new UnitOfWork())
                {
                    var summary = unitOfWork.Reservations.BeginReservation(userId, dbModel);

                    return JsonFactory.CreateJsonMessage(summary, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }

        /// <summary>
        /// Checks the status of the reservation. Updates last pending time, if still pending.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Reservation/CheckStatus/")]
        [ResponseType(typeof(OutgoingMinimalReservationGroup))]
        [Authorize]
        public HttpResponseMessage CheckStatus(int id)
        {
            return ErrorFactory.Handle(() =>
            {
                var userId = User?.Identity?.GetUserId();

                if (string.IsNullOrWhiteSpace(userId))
                {
                    throw new Exception();
                }

                using (var unitOfWork = new UnitOfWork())
                {
                    var reservation = unitOfWork.Reservations.CheckStatus(userId, id);


                    if(reservation == null)
                    {
                        return JsonFactory.CreateJsonMessage(null, HttpStatusCode.OK, this.Request);
                    }

                    unitOfWork.Complete();

                    var outgoingReservation = OutgoingMinimalReservationGroup.Parse(reservation);

                    return JsonFactory.CreateJsonMessage(outgoingReservation, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }

        /// <summary>
        /// Cancels a pending reservation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/Reservation/DeletePending")]
        [ResponseType(typeof(OutgoingMessage))]
        [Authorize]
        public HttpResponseMessage CancelPendingReservation(int id)
        {
            return ErrorFactory.Handle(() =>
            {
                var userId = User?.Identity?.GetUserId();

                if (string.IsNullOrWhiteSpace(userId))
                {
                    throw new Exception();
                }

                using (var unitOfWork = new UnitOfWork())
                {
                    unitOfWork.Reservations.DeletePending(userId, id);

                    unitOfWork.Complete();

                    return JsonFactory.CreateJsonMessage(new OutgoingMessage { Message = "Okay" }, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }

        /// <summary>
        /// Finish the purchase process of a transaction
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Reservation/Process")]
        [ResponseType(typeof(OutgoingReservationGroup))]
        [Authorize]
        public HttpResponseMessage ProcessTransaction(IncomingProcessTransaction model)
        {
            return ErrorFactory.Handle(() =>
            {
                var userId = User?.Identity?.GetUserId();

                if (string.IsNullOrWhiteSpace(userId))
                {
                    throw new Exception();
                }

                var context = new PoolReservationEntities();

                using (var unitOfWork = new UnitOfWork(context))
                {
                    PrepareAndGetReservationForProcessing_Result reservation = null;

                    try
                    {
                        reservation = context.PrepareAndGetReservationForProcessing(model.ReservationId)?.FirstOrDefault();
                    }
                    catch (Exception)
                    {
                        context.ChangeProcessingStatusToPending(model.ReservationId);
                    }
                    
                  

                    if (reservation == null)
                    {
                        try
                        {
                            context.ChangeProcessingStatusToPending(model.ReservationId);
                        }
                        catch (Exception)
                        {

                        }

                        return JsonFactory.CreateJsonMessage(null, HttpStatusCode.NotFound, this.Request);
                    }

                    if (reservation.StatusId != Convert.ToInt32(ReservationGroupStatusEnum.PROCESSING))
                    {
                        try
                        {
                            context.ChangeProcessingStatusToPending(model.ReservationId);
                        }
                        catch (Exception)
                        {

                        }

                        return JsonFactory.CreateJsonMessage(new OutgoingMessage { Action = "wrongStatus" }, HttpStatusCode.NotFound, this.Request);
                    }

                    ReservationGroup reservationDBObject = null;


                    try
                    {
                        reservationDBObject = unitOfWork.Reservations.GetReservationWithItems(userId, model.ReservationId);

                        if (reservationDBObject == null)
                        {
                            return JsonFactory.CreateJsonMessage(null, HttpStatusCode.NotFound, this.Request);
                        }
                    }
                    catch (Exception)
                    {
                        context.ChangeProcessingStatusToPending(model.ReservationId);
                    }
                    

                    if (reservationDBObject.StatusId != Convert.ToInt32(ReservationGroupStatusEnum.PROCESSING))
                    {

                        try
                        {
                            context.ChangeProcessingStatusToPending(model.ReservationId);
                        }
                        catch (Exception)
                        {

                        }

                        return JsonFactory.CreateJsonMessage(new OutgoingMessage { Action = "wrongStatus" }, HttpStatusCode.NotFound, this.Request);
                    }

                    ReservationTransaction resTransaction = null;
                    int stripeModifiedPriceInCents = 0;
                    try
                    {
                        decimal priceToCharge = 0M;

                        foreach (var item in reservationDBObject.ReserveItems)
                        {
                            if (item.IsDeleted == true)
                            {
                                continue;
                            }

                            priceToCharge += item.FinalPrice;
                        }



                        decimal stripeModifiedPrice = priceToCharge * 100.0M;

                        stripeModifiedPriceInCents = (int)Math.Round(stripeModifiedPrice); //Must Update Repo If Changed.  Rounds to the nearest penny. 

                        if (stripeModifiedPriceInCents <= 50)
                        {
                            return JsonFactory.CreateJsonMessage(new OutgoingMessage { Action = "noPriceToCharge" }, HttpStatusCode.NotFound, this.Request);
                        }

                        resTransaction = unitOfWork.Reservations.AddStripeChargeToPendingReservation(userId, userId, model.ReservationId, model.StripeTokenId, priceToCharge);

                        unitOfWork.Complete();
                    }
                    catch(Exception e)
                    {
                        context.ChangeProcessingStatusToPending(model.ReservationId);
                    }




                    string chargeId = null;
                    try
                    {

                        if (resTransaction == null)
                        {
                            throw new Exception();
                        }

                        var myCharge = new StripeChargeCreateOptions();

                        // always set these properties
                        myCharge.Amount = stripeModifiedPriceInCents;
                        myCharge.Currency = "usd";

                        // set this if you want to
                        myCharge.Description = "JustMosey Reservation";

                        myCharge.SourceTokenOrExistingSourceId = model.StripeTokenId;

                        // (not required) set this to false if you don't want to capture the charge yet - requires you call capture later
                        myCharge.Capture = true;

                        var chargeService = new StripeChargeService();
                        StripeCharge stripeCharge = chargeService.Create(myCharge);

                        chargeId = stripeCharge.Id;

                        unitOfWork.Reservations.UpdateStripeSuccessfulChargeOnPendingReservation(userId, userId, resTransaction.Id, chargeId);

                        unitOfWork.Complete();
                    }
                    catch (Exception e)
                    {
                        try
                        {

                            var refundService = new StripeRefundService();

                            StripeRefund refund = refundService.Create(chargeId, new StripeRefundCreateOptions()
                            {
                                Amount = stripeModifiedPriceInCents,
                                Reason = StripeRefundReasons.Unknown
                            });
                        }
                        catch (Exception)
                        {
                        }

                        try
                        {
                            unitOfWork.Reservations.RefundStripeSuccessfulChargeOnPendingReservation(userId, userId, resTransaction.Id);
                            unitOfWork.Complete();
                        }
                        catch (Exception)
                        {
                            
                        }

                        try
                        {

                        }
                        catch (Exception)
                        {
                            context.ChangeProcessingStatusToPending(model.ReservationId);
                        }

                        return JsonFactory.CreateJsonMessage(new OutgoingMessage { Action = "unknownErrorAfterProcessing" }, HttpStatusCode.InternalServerError, this.Request);
                    }

                    try
                    {
                        var updatedReservationDBObject = unitOfWork.Reservations.GetReservationWithItems(userId, model.ReservationId);

                        var outgoingRes = OutgoingReservationGroup.Parse(updatedReservationDBObject);

                        return JsonFactory.CreateJsonMessage(outgoingRes, HttpStatusCode.OK, this.Request);
                    }
                    catch (Exception)
                    {
                        return JsonFactory.CreateJsonMessage(null, HttpStatusCode.OK, this.Request);
                    }
                   
                }
            }, this.Request);
        }


        /// <summary>
        /// Gets the hotel reservations.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Reservation/Hotel/")]
        [ResponseType(typeof(ICollection<OutgoingMinimalReservationGroupExtra>))]
        [Authorize]
        public HttpResponseMessage GetHotelReservations(IncomingGetHotelReservations model)
        {
            return ErrorFactory.Handle(() =>
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var userId = User?.Identity?.GetUserId();

                    if (string.IsNullOrWhiteSpace(userId))
                    {
                        throw new Exception();
                    }

                    if(model == null)
                    {
                        throw new InvalidModelException();
                    }

                    var reservations = unitOfWork.Reservations.GetReservationsForHotel(userId, model.HotelId, model.StartDate, model.EndDate);


                    var outgoingReservations = reservations?.Select(x => OutgoingMinimalReservationGroupExtra.Parse(x));

                    return JsonFactory.CreateJsonMessage(outgoingReservations, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }


        /// <summary>
        /// Gets the user reservations.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Reservation/User/")]
        [ResponseType(typeof(ICollection<OutgoingMinimalReservationGroupExtra>))]
        [Authorize]
        public HttpResponseMessage GetUserReservations(IncomingGetUsersReservations model)
        {
            return ErrorFactory.Handle(() =>
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var userId = User?.Identity?.GetUserId();

                    if (string.IsNullOrWhiteSpace(userId))
                    {
                        throw new Exception();
                    }

                    if (model == null)
                    {
                        throw new InvalidModelException();
                    }

                    var reservations = unitOfWork.Reservations.GetReservationsForUser(userId, model.UserId, model.StartDate, model.EndDate);


                    var outgoingReservations = reservations?.Select(x => OutgoingMinimalReservationGroupExtra.Parse(x));

                    return JsonFactory.CreateJsonMessage(outgoingReservations, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }


        /// <summary>
        /// Gets the reservation.
        /// </summary>
        /// <param name="resId">The reservation identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Reservation/")]
        [ResponseType(typeof(OutgoingReservationGroup))]
        [Authorize]
        public HttpResponseMessage GetReservation(int resId)
        {
            return ErrorFactory.Handle(() =>
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var userId = User?.Identity?.GetUserId();

                    if (string.IsNullOrWhiteSpace(userId))
                    {
                        throw new Exception();
                    }

                    var reservation = unitOfWork.Reservations.GetReservationWithItems(userId, resId);

                    var outgoingReservations = OutgoingReservationGroup.Parse(reservation);

                    return JsonFactory.CreateJsonMessage(outgoingReservations, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }


        /// <summary>
        /// Searches the user reservations.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Reservation/User/Search")]
        [ResponseType(typeof(ICollection<OutgoingMinimalReservationGroupExtra>))]
        [Authorize]
        public HttpResponseMessage SearchUserReservations(IncomingSearchUserReservation model)
        {
            return ErrorFactory.Handle(() =>
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var userId = User?.Identity?.GetUserId();

                    if (string.IsNullOrWhiteSpace(userId))
                    {
                        throw new Exception();
                    }

                    var reservations = unitOfWork.Reservations.SearchUserReservations(userId, model.UserId, model.Query, model.SearchType, model.StartDate, model.EndDate).ToList();

                    var outgoingReservations = reservations.Select(x => OutgoingMinimalReservationGroupExtra.Parse(x)).ToList();

                    return JsonFactory.CreateJsonMessage(outgoingReservations, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }

        /// <summary>
        /// Searches the hotel reservations.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Reservation/Hotel/Search")]
        [ResponseType(typeof(ICollection<OutgoingMinimalReservationGroupExtra>))]
        [Authorize]
        public HttpResponseMessage SearchHotelReservations(IncomingSearchHotelReservation model)
        {
            return ErrorFactory.Handle(() =>
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var userId = User?.Identity?.GetUserId();

                    if (string.IsNullOrWhiteSpace(userId))
                    {
                        throw new Exception();
                    }

                    var reservations = unitOfWork.Reservations.SearchHotelReservations(userId, model.HotelId, model.Query, model.SearchType, model.StartDate, model.EndDate).ToList();

                    var outgoingReservations = reservations.Select(x => OutgoingMinimalReservationGroupExtra.Parse(x)).ToList();

                    return JsonFactory.CreateJsonMessage(outgoingReservations, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }

        /// <summary>
        /// Searches all reservations.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Reservation/Search")]
        [ResponseType(typeof(ICollection<OutgoingMinimalReservationGroupExtra>))]
        [Authorize]
        public HttpResponseMessage SearchAllReservations(IncomingSearchAllReservation model)
        {
            return ErrorFactory.Handle(() =>
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var userId = User?.Identity?.GetUserId();

                    if (string.IsNullOrWhiteSpace(userId))
                    {
                        throw new Exception();
                    }

                    var reservations = unitOfWork.Reservations.SearchAllReservations(userId, model.Query, model.SearchType, model.StartDate, model.EndDate).ToList();

                    var outgoingReservations = reservations.Select(x => OutgoingMinimalReservationGroupExtra.Parse(x)).ToList();

                    return JsonFactory.CreateJsonMessage(outgoingReservations, HttpStatusCode.OK, this.Request);
                }
            }, this.Request);
        }



    }
}
