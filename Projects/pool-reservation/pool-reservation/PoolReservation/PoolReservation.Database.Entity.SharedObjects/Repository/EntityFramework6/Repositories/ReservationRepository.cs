using PoolReservation.Database.Entity.SharedObjects.Repository.Base.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PoolReservation.Database.Entity.Model.Reservations;
using PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6.Messages;
using PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6.Exceptions;
using PoolReservation.SharedObjects.Model.Exceptions.Validation;

namespace PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6.Repositories
{
    public class ReservationRepository : Repository, IReservationRepository
    {
        public ReservationRepository(PoolReservationEntities context, UnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }


        public BeginTransactionGroupSummary BeginReservation(string currentUserId, BeginReservation reservationDetails)
        {
            var currentSummary = new BeginTransactionGroupSummary
            {
                OriginalObject = reservationDetails
            };

            if ((reservationDetails?.Items?.Count() ?? 0) < 1)
            {
                currentSummary.Status.FailedCode = BeginTransactionGroupFailedCodes.NO_ITEMS_IN_RESERVATION;

                throw new BeginTransactionException { Summary = currentSummary };
            }





            using (var dbTransaction = this.dbContext.Database.BeginTransaction())
            {
                try
                {
                    var currentUser = this.unitOfWork.Users.GetUserById(reservationDetails.UserId, reservationDetails.UserId);

                    if (currentUser == null)
                    {
                        currentSummary.Status.FailedCode = BeginTransactionGroupFailedCodes.USER_NOT_FOUND;
                        throw new BeginTransactionException { Summary = currentSummary };
                    }

                    var currentHotel = this.unitOfWork.Hotels.GetHotelById(currentUser.Id, reservationDetails.HotelId);

                    if (currentHotel == null)
                    {
                        currentSummary.Status.FailedCode = BeginTransactionGroupFailedCodes.HOTEL_NOT_FOUND;
                        throw new BeginTransactionException { Summary = currentSummary };
                    }

                    if (currentUser.Id == currentUserId)
                    {
                        var auth = this.CanUserAddReservation(currentUser.Id, currentHotel.Id);

                        if (auth == false)
                        {
                            currentSummary.Status.FailedCode = BeginTransactionGroupFailedCodes.NOT_AUTHORIZED;
                            throw new BeginTransactionException { Summary = currentSummary };
                        }
                    }
                    else
                    {
                        var auth = this.CanUserAddOtherReservations(currentUserId, currentHotel.Id);

                        if (auth == false)
                        {
                            currentSummary.Status.FailedCode = BeginTransactionGroupFailedCodes.NOT_AUTHORIZED;
                            throw new BeginTransactionException { Summary = currentSummary };
                        }
                    }



                    var currentTaxRate = currentHotel.TaxRate;

                    var theNewReservationGroup = new ReservationGroup();

                    theNewReservationGroup.HotelId = reservationDetails.HotelId;
                    theNewReservationGroup.UserId = reservationDetails.UserId;

                    theNewReservationGroup.StatusId = Convert.ToInt32(ReservationGroupStatusEnum.PENDING);
                    theNewReservationGroup.StatusDate = DateTime.UtcNow;
                    theNewReservationGroup.StatusGuid = null;

                    theNewReservationGroup.DateCreated = DateTime.UtcNow;


                    this.dbContext.ReservationGroup.Add(theNewReservationGroup);
                    currentSummary.Status.Valid = true;
                    try
                    {
                        this.dbContext.SaveChanges();
                    }
                    catch (Exception)
                    {
                        currentSummary.Status.FailedCode = BeginTransactionGroupFailedCodes.UNKNOWN_INVALID_GROUP;
                        throw new BeginTransactionException { Summary = currentSummary };
                    }



                    currentSummary.ReservationGroupId = theNewReservationGroup.Id;
                    currentSummary.Status.Completed = true;

                    decimal FinalOverallPrice = 0.0M;

                    foreach (var item in reservationDetails.Items)
                    {
                        var currentItemSummary = new BeginTransactionItemSummary { OriginalObject = item };
                        currentSummary.ItemSummaries.Add(currentItemSummary);

                        if (item == null)
                        {
                            currentItemSummary.Status.FailedCode = BeginTransactionItemFailedCodes.NULL_ITEM;
                            throw new BeginTransactionException { Summary = currentSummary };
                        }

                        if (item.Date == null)
                        {
                            currentItemSummary.Status.FailedCode = BeginTransactionItemFailedCodes.NULL_DAY;
                            throw new BeginTransactionException { Summary = currentSummary };
                        }

                        if (item.Date.Date.ToUniversalTime().Equals(DateTime.UtcNow.Date))
                        {
                            currentItemSummary.Status.FailedCode = BeginTransactionItemFailedCodes.INVALID_SAME_DAY;
                            throw new BeginTransactionException { Summary = currentSummary };
                        }

                        var itemToReserve = this.unitOfWork.VenueItems.GetVenueItemById(currentUser.Id, item.ItemId);

                        if (itemToReserve == null)
                        {
                            currentItemSummary.Status.FailedCode = BeginTransactionItemFailedCodes.ITEM_NOT_FOUND;
                            throw new BeginTransactionException { Summary = currentSummary };
                        }

                        if (item.Quantity <= 0)
                        {
                            currentItemSummary.Status.FailedCode = BeginTransactionItemFailedCodes.QUANTITY_TOO_SMALL;
                            throw new BeginTransactionException { Summary = currentSummary };
                        }

                        var newItem = new ReserveItems();

                        newItem.ItemId = item.ItemId;
                        newItem.Quantity = item.Quantity;
                        newItem.PricePreTax = itemToReserve.Price;
                        newItem.TaxRate = (decimal)currentTaxRate;
                        newItem.DateReservedFor = item.Date;
                        newItem.FinalPrice = (newItem.PricePreTax + (1.0M * newItem.TaxRate)) * item.Quantity;

                        theNewReservationGroup.ReserveItems.Add(newItem);

                        currentItemSummary.Status.Valid = true;

                        try
                        {
                            this.dbContext.SaveChanges();
                        }
                        catch (Exception)
                        {
                            currentItemSummary.Status.FailedCode = BeginTransactionItemFailedCodes.UNKNOWN_ITEM_EXCEPTION;
                            throw new BeginTransactionException { Summary = currentSummary };
                        }

                        currentItemSummary.ReservationItemId = newItem.Id;
                        currentItemSummary.Status.Completed = true;
                        currentItemSummary.FinalPrice = newItem.FinalPrice;

                        FinalOverallPrice += newItem.FinalPrice;
                    }

                    dbTransaction.Commit();

                    FinalOverallPrice = Math.Round(FinalOverallPrice * 100.0M) / 100.0M; //Must update controller if changed. Rounds to the nearest penny. 

                    currentSummary.FinalPrice = FinalOverallPrice;
                    return currentSummary;
                }
                catch (Exception)
                {
                    dbTransaction.Rollback();

                    if (currentSummary != null)
                    {
                        currentSummary.ReservationGroupId = null;
                    }

                    throw new BeginTransactionException { Summary = currentSummary };
                }
            }
        }

        public ReservationGroup CheckStatus(string userId, int id)
        {
            var auth = this.CanUserEditReservation(userId, id);

            if (auth == false)
            {
                throw new UnauthorizedAccessException();
            }

            this.dbContext.UpdateStatusDate(id);  // Change this, add expected status!!!!

            var reservation = this.dbContext.ReservationGroup
                .Include(x => x.ReserveItems)
                .Include(x => x.TransactionsForReservations)
                .FirstOrDefault(x => x.Id == id);

            return reservation;
        }

        public void DeletePending(string userId, int id)
        {
            var auth = this.CanUserDeleteReservation(userId, id);

            if (auth == false)
            {
                throw new UnauthorizedAccessException();
            }

            this.dbContext.DeletePendingReservation(id);
        }

        public ReservationGroup GetReservationWithItems(string userId, int id)
        {
            var auth = this.CanUserViewReservation(userId, id);

            if (auth == false)
            {
                throw new UnauthorizedAccessException();
            }

            var reservation = this.dbContext.ReservationGroup
                .Include(x => x.ReserveItems)
                .Include(x => x.ReserveItems.Select(y => y.VenueItems))
                .Include(x => x.ReserveItems.Select(y => y.VenueItems.ItemTypes))
                .Include(x => x.ReserveItems.Select(y => y.VenueItems.Icons))
                .Include(x => x.ReserveItems.Select(y => y.VenueItems.Icons.Pictures))
                .Include(x => x.ReserveItems.Select(y => y.VenueItems.Icons.Pictures.PictureResolutions))
                .Include(x => x.ReserveItems.Select(y => y.VenueItems.Icons.Pictures.PictureResolutions.Select(z => z.PictureUrls)))
                .FirstOrDefault(x => x.Id == id);

            return reservation;
        }


        public ReservationTransaction AddStripeChargeToPendingReservation(string currentUserId, string userIdFor, int reservationId, string tokenId, decimal amount)
        {
            var stripePaymentDetails = new TransactionStripeDetails
            {
                TokenId = tokenId
            };

            var transactionPaymentDetails = new TransactionPaymentDetails
            {
                TransactionStripeDetails = stripePaymentDetails
            };

            var resTransaction = new ReservationTransaction
            {
                TransactionPaymentDetails = transactionPaymentDetails,
                DateCreated = DateTime.UtcNow,
                DateCompleted = DateTime.UtcNow,
                UserTransactionCompletedById = currentUserId,
                UserTransactionIsForId = userIdFor,
                TransactionType = Convert.ToInt32(TransactionTypeEnum.CHARGE),
                AmountCharged = amount,
                TransactionStatusId = Convert.ToInt32(TransactionStatusEnum.PENDING)
            };


            var trans = new TransactionsForReservations
            {
                DateLinked = DateTime.UtcNow,
                ReservationTransaction = resTransaction,
            };


            var reservation = this.dbContext.ReservationGroup.FirstOrDefault(x => x.Id == reservationId);

            if (reservation.StatusId != Convert.ToInt32(ReservationGroupStatusEnum.PROCESSING))
            {
                throw new Exception();
            }

            reservation.StatusDate = DateTime.UtcNow;

            trans.ReservationGroupId = reservation.Id;

            this.dbContext.TransactionsForReservations.Add(trans);

            return resTransaction;
        }

        public ReservationTransaction UpdateStripeSuccessfulChargeOnPendingReservation(string currentUserId, string userIdFor, Guid resTransId, string chargeId)
        {
            var resTrans = this.dbContext.ReservationTransaction?.Include(x => x.TransactionPaymentDetails.TransactionStripeDetails).FirstOrDefault(x => x.Id == resTransId);

            var stripeDetails = resTrans?.TransactionPaymentDetails?.TransactionStripeDetails;

            if (stripeDetails == null)
            {
                throw new Exception();
            }

            stripeDetails.ChargeId = chargeId;

            resTrans.DateCompleted = DateTime.UtcNow;
            resTrans.TransactionStatusId = Convert.ToInt32(TransactionStatusEnum.SUCCESSFUL);


            var group = resTrans.TransactionsForReservations.FirstOrDefault().ReservationGroup;


            group.StatusId = Convert.ToInt32(ReservationGroupStatusEnum.COMPLETED);
            group.StatusDate = DateTime.UtcNow;

            return resTrans;
        }

        public ReservationTransaction RefundStripeSuccessfulChargeOnPendingReservation(string currentUserId, string userIdFor, Guid resTransId)
        {

            var resTrans = this.dbContext.ReservationTransaction?.Include(x => x.TransactionPaymentDetails.TransactionStripeDetails).FirstOrDefault(x => x.Id == resTransId);

            if (resTrans == null)
            {
                throw new Exception();
            }

            resTrans.DateCompleted = DateTime.UtcNow;
            resTrans.TransactionStatusId = Convert.ToInt32(TransactionStatusEnum.CANCELLED);

            return resTrans;
        }

        public IEnumerable<ReservationGroup> GetReservationsForHotel(string userId, int hotelId, DateTime? startDate, DateTime? endDate)
        {

            var auth = this.CanUserViewHotelReservations(userId, hotelId);

            if (auth == false)
            {
                throw new UnauthorizedAccessException();
            }

            if (startDate != null && endDate != null)
            {
                return this.dbContext.ReservationGroup
                    .Include(x => x.AspNetUsers)
                    .Include(x => x.ReservationGroupStatus)
                    .Include(x => x.Hotels)
                    .Include(x => x.ReserveItems)
                    .Where(x => x.HotelId == hotelId && x.ReserveItems.Any(y => y.DateReservedFor >= startDate && y.DateReservedFor <= endDate));
            }
            else if (startDate == null && endDate == null)
            {
                return this.dbContext.ReservationGroup
                    .Include(x => x.AspNetUsers)
                    .Include(x => x.ReservationGroupStatus)
                    .Include(x => x.Hotels)
                    .Include(x => x.ReserveItems)
                    .Where(x => x.HotelId == hotelId);
            }
            else
            {
                throw new Exception();
            }


        }

        public IEnumerable<ReservationGroup> GetReservationsForUser(string currentUserId, string userToGet, DateTime? startDate, DateTime? endDate)
        {

            var auth = this.CanUserViewOtherUsersReservations(currentUserId, userToGet);

            if (auth == false)
            {
                throw new UnauthorizedAccessException();
            }

            if (startDate != null && endDate != null)
            {
                return this.dbContext.ReservationGroup
                    .Include(x => x.AspNetUsers)
                    .Include(x => x.ReservationGroupStatus)
                    .Include(x => x.Hotels)
                    .Include(x => x.ReserveItems)
                    .Where(x => x.UserId == userToGet && x.ReserveItems.Any(y => y.DateReservedFor >= startDate && y.DateReservedFor <= endDate));
            }
            else if (startDate == null && endDate == null)
            {
                return this.dbContext.ReservationGroup
                    .Include(x => x.AspNetUsers)
                    .Include(x => x.ReservationGroupStatus)
                    .Include(x => x.Hotels)
                    .Include(x => x.ReserveItems)
                    .Where(x => x.UserId == userToGet);
            }
            else
            {
                throw new Exception();
            }



        }

        public bool CanUserViewReservation(string userId, int resId)
        {
            var reservation = this.dbContext.ReservationGroup.FirstOrDefault(x => x.Id == resId);

            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            var permissions = user?.SitePermissions;

            if (reservation == null || permissions == null)
            {
                return false;
            }

            var hotelPermissions = this.unitOfWork.Hotels.GetPermissionsForUserAtHotel(userId, reservation.HotelId);


            if (reservation.UserId == userId)
            {
                if (hotelPermissions?.PersonalReservationPermissions?.View == true)
                {
                    return true;
                }

                if (permissions?.PersonalReservationsPermissions?.View == true)
                {
                    return true;
                }

                return false;
            }
            else
            {
                if (hotelPermissions?.OtherReservationsPermissions?.View == true)
                {
                    return true;
                }

                if (permissions?.OtherReservationsPermissions?.View == true)
                {
                    return true;
                }

                return false;
            }
        }

        public bool CanUserDeleteReservation(string userId, int resId)
        {
            var reservation = this.dbContext.ReservationGroup.FirstOrDefault(x => x.Id == resId);

            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            var permissions = user?.SitePermissions;

            if (reservation == null || permissions == null)
            {
                return false;
            }

            var hotelPermissions = this.unitOfWork.Hotels.GetPermissionsForUserAtHotel(userId, reservation.HotelId);


            if (reservation.UserId == userId)
            {
                if (hotelPermissions?.PersonalReservationPermissions?.Delete == true)
                {
                    return true;
                }

                if (permissions?.PersonalReservationsPermissions?.Delete == true)
                {
                    return true;
                }

                return false;
            }
            else
            {
                if (hotelPermissions?.OtherReservationsPermissions?.Delete == true)
                {
                    return true;
                }

                if (permissions?.OtherReservationsPermissions?.Delete == true)
                {
                    return true;
                }

                return false;
            }
        }

        public bool CanUserAddReservation(string userId, int hotelId)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            var permissions = user?.SitePermissions;

            if (permissions == null)
            {
                return false;
            }

            var hotelPermissions = this.unitOfWork.Hotels.GetPermissionsForUserAtHotel(userId, hotelId);

            if (hotelPermissions?.PersonalReservationPermissions?.Add == true)
            {
                return true;
            }

            if (permissions?.PersonalReservationsPermissions?.Add == true)
            {
                return true;
            }

            return false;
        }

        public bool CanUserEditReservation(string userId, int resId)
        {
            var reservation = this.dbContext.ReservationGroup.FirstOrDefault(x => x.Id == resId);

            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            var permissions = user?.SitePermissions;

            if (reservation == null || permissions == null)
            {
                return false;
            }

            var hotelPermissions = this.unitOfWork.Hotels.GetPermissionsForUserAtHotel(userId, reservation.HotelId);


            if (reservation.UserId == userId)
            {
                if (hotelPermissions?.PersonalReservationPermissions?.Edit == true)
                {
                    return true;
                }

                if (permissions?.PersonalReservationsPermissions?.Edit == true)
                {
                    return true;
                }

                return false;
            }
            else
            {
                if (hotelPermissions?.OtherReservationsPermissions?.Edit == true)
                {
                    return true;
                }

                if (permissions?.OtherReservationsPermissions?.Edit == true)
                {
                    return true;
                }

                return false;
            }
        }

        public bool CanUserViewHotelReservations(string userId, int hotelId)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            var permissions = user?.SitePermissions;

            if (permissions == null)
            {
                return false;
            }

            var hotelPermissions = this.unitOfWork.Hotels.GetPermissionsForUserAtHotel(userId, hotelId);

            if (hotelPermissions?.OtherReservationsPermissions?.Add == true)
            {
                return true;
            }

            if (permissions?.OtherReservationsPermissions?.Add == true)
            {
                return true;
            }

            return false;
        }

        public bool CanUserViewPersonalReservations(string currentUserId, string userIdToView)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(currentUserId);

            var permissions = user?.SitePermissions;

            if (permissions == null)
            {
                return false;
            }

            if (permissions?.OtherReservationsPermissions?.Add == true)
            {
                return true;
            }

            return false;
        }

        public bool CanUserAddOtherReservations(string userId, int hotelId)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(userId);

            var permissions = user?.SitePermissions;

            if (permissions == null)
            {
                return false;
            }

            var hotelPermissions = this.unitOfWork.Hotels.GetPermissionsForUserAtHotel(userId, hotelId);

            if (hotelPermissions?.OtherReservationsPermissions?.Add == true)
            {
                return true;
            }

            if (permissions?.OtherReservationsPermissions?.Add == true)
            {
                return true;
            }

            return false;
        }

        public bool CanUserViewOtherUsersReservations(string currentUserId, string userIdToView)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(currentUserId);

            var permissions = user?.SitePermissions;

            if (permissions == null)
            {
                return false;
            }

            if (currentUserId == userIdToView)
            {
                return permissions?.PersonalReservationsPermissions?.View ?? false;
            }
            else if (permissions?.OtherReservationsPermissions?.View == true)
            {
                return true;
            }

            return false;
        }

        public bool CanUserViewOtherUsersReservations(string currentUserId)
        {
            return this.CanUserViewOtherUsersReservations(currentUserId, null);
        }

        public IEnumerable<ReservationGroup> SearchUserReservations(string currentUserId, string userToGet, string query, ReservationSearchTypeEnum? searchType, DateTime startDate, DateTime endDate)
        {
            var auth = this.CanUserViewOtherUsersReservations(currentUserId, userToGet);

            if (auth == false)
            {
                throw new UnauthorizedAccessException();
            }

            if (startDate > endDate)
            {
                throw new Exception();
            }

            var fixedQueryString = query?.Trim()?.ToLower();

            startDate = startDate.Date;
            endDate = endDate.Date;

            var currentState =
                this.dbContext.ReservationGroup
                    .Include(x => x.AspNetUsers)
                    .Include(x => x.ReservationGroupStatus)
                    .Include(x => x.Hotels)
                    .Include(x => x.ReserveItems)
                    .Where(x => x.UserId == userToGet && x.ReserveItems.Any(y => y.DateReservedFor >= startDate && y.DateReservedFor <= endDate));

            if (string.IsNullOrWhiteSpace(fixedQueryString))
            {
                return currentState;
            }

            if (searchType == ReservationSearchTypeEnum.HotelId)
            {
                int hotelId = -1;

                var result = int.TryParse(fixedQueryString, out hotelId);

                if (result == false)
                {
                    throw new Exception();
                }

                currentState = currentState.Where(x => x.HotelId == hotelId);
            }
            else if (searchType == ReservationSearchTypeEnum.ReservationId)
            {
                int id = -1;

                var result = int.TryParse(fixedQueryString, out id);

                if (result == false)
                {
                    throw new Exception();
                }

                currentState = currentState.Where(x => x.Id == id);
            }
            else if (searchType == ReservationSearchTypeEnum.UserEmail)
            {
                currentState = currentState.Where(x => x.AspNetUsers.Email.ToLower().Contains(fixedQueryString));
            }
            else if (searchType == ReservationSearchTypeEnum.UserId)
            {
                currentState = currentState.Where(x => x.UserId.ToLower().Contains(fixedQueryString));
            }
            else
            {

            }

            return currentState;


        }

        public IEnumerable<ReservationGroup> SearchHotelReservations(string currentUserId, int hotelId, string query, ReservationSearchTypeEnum? searchType, DateTime startDate, DateTime endDate)
        {
            var auth = this.CanUserViewHotelReservations(currentUserId, hotelId);

            if (auth == false)
            {
                throw new UnauthorizedAccessException();
            }

            if (startDate > endDate)
            {
                throw new Exception();
            }

            var fixedQueryString = query?.Trim()?.ToLower();

            startDate = startDate.Date;
            endDate = endDate.Date;


            var currentState =
                this.dbContext.ReservationGroup
                    .Include(x => x.AspNetUsers)
                    .Include(x => x.ReservationGroupStatus)
                    .Include(x => x.Hotels)
                    .Include(x => x.ReserveItems)
                    .Where(x => x.HotelId == hotelId && x.ReserveItems.Any(y => y.DateReservedFor >= startDate && y.DateReservedFor <= endDate));

            if (string.IsNullOrWhiteSpace(fixedQueryString))
            {
                return currentState;
            }

            if (searchType == ReservationSearchTypeEnum.HotelId)
            {
                //Nothing needed, already handled above
            }
            else if (searchType == ReservationSearchTypeEnum.ReservationId)
            {
                int id = -1;

                var result = int.TryParse(fixedQueryString, out id);

                if (result == false)
                {
                    throw new Exception();
                }

                currentState = currentState.Where(x => x.Id == id);
            }
            else if (searchType == ReservationSearchTypeEnum.UserEmail)
            {
                currentState = currentState.Where(x => x.AspNetUsers.Email.ToLower().Contains(fixedQueryString));
            }
            else if (searchType == ReservationSearchTypeEnum.UserId)
            {
                currentState = currentState.Where(x => x.UserId.ToLower().Contains(fixedQueryString));
            }
            else
            {

            }

            return currentState;
        }

        public IEnumerable<ReservationGroup> SearchAllReservations(string currentUserId, string query, ReservationSearchTypeEnum? searchType, DateTime startDate, DateTime endDate)
        {
            var auth = this.CanUserViewOtherUsersReservations(currentUserId);

            if (auth == false)
            {
                throw new UnauthorizedAccessException();
            }

            if (startDate > endDate)
            {
                throw new Exception();
            }

            var fixedQueryString = query?.Trim()?.ToLower();

            startDate = startDate.Date;
            endDate = endDate.Date;

            var currentState =
                this.dbContext.ReservationGroup
                    .Include(x => x.AspNetUsers)
                    .Include(x => x.ReservationGroupStatus)
                    .Include(x => x.Hotels)
                    .Include(x => x.ReserveItems)
                    .Where(x => x.ReserveItems.Any(y => y.DateReservedFor >= startDate && y.DateReservedFor <= endDate));

            if (string.IsNullOrWhiteSpace(fixedQueryString))
            {
                return currentState;
            }

            if (searchType == ReservationSearchTypeEnum.HotelId)
            {
                int hotelId = -1;

                var result = int.TryParse(fixedQueryString, out hotelId);

                if (result == false)
                {
                    throw new Exception();
                }

                currentState = currentState.Where(x => x.HotelId == hotelId);
            }
            else if (searchType == ReservationSearchTypeEnum.ReservationId)
            {
                int id = -1;

                var result = int.TryParse(fixedQueryString, out id);

                if (result == false)
                {
                    throw new Exception();
                }

                currentState = currentState.Where(x => x.Id == id);
            }
            else if (searchType == ReservationSearchTypeEnum.UserEmail)
            {
                currentState = currentState.Where(x => x.AspNetUsers.Email.ToLower().Contains(fixedQueryString));
            }
            else if (searchType == ReservationSearchTypeEnum.UserId)
            {
                currentState = currentState.Where(x => x.UserId.ToLower().Contains(fixedQueryString));
            }
            else
            {

            }

            return currentState;
        }
    }
}
