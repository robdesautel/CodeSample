using PoolReservation.Database.Entity.Model.Reservations;
using PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.SharedObjects.Repository.Base.Repositories
{
    public interface IReservationRepository : IRepository
    {

        bool CanUserViewReservation(string userId, int resId);

        bool CanUserDeleteReservation(string userId, int resId);

        bool CanUserAddReservation(string userId, int hotelId);

        bool CanUserEditReservation(string userId, int resId);

        bool CanUserViewHotelReservations(string userId, int hotelId);

        bool CanUserViewPersonalReservations(string currentUserId, string userIdToView);

        bool CanUserViewOtherUsersReservations(string currentUserId, string userIdToView);
        bool CanUserViewOtherUsersReservations(string currentUserId);

        bool CanUserAddOtherReservations(string userId, int hotelId);

        BeginTransactionGroupSummary BeginReservation(string currentUserId, BeginReservation reservationDetails);

        ReservationGroup CheckStatus(string userId, int id);

        void DeletePending(string userId, int id);

        ReservationGroup GetReservationWithItems(string userId, int id);

        ReservationTransaction AddStripeChargeToPendingReservation(string currentUserId, string userIdFor, int reservationId, string tokenId, decimal amount);

        ReservationTransaction UpdateStripeSuccessfulChargeOnPendingReservation(string currentUserId, string userIdFor, Guid resTransId, string chargeId);

        ReservationTransaction RefundStripeSuccessfulChargeOnPendingReservation(string currentUserId, string userIdFor, Guid resTransId);

        IEnumerable<ReservationGroup> GetReservationsForHotel(string userId, int hotelId, DateTime? startDate, DateTime? endDate);

        IEnumerable<ReservationGroup> GetReservationsForUser(string currentUserId, string userToGet, DateTime? startDate, DateTime? endDate);

        IEnumerable<ReservationGroup> SearchUserReservations(string currentUserId, string userToGet, string query, ReservationSearchTypeEnum? searchType, DateTime startDate, DateTime endDate);

        IEnumerable<ReservationGroup> SearchHotelReservations(string currentUserId, int hotelId, string query, ReservationSearchTypeEnum? searchType, DateTime startDate, DateTime endDate);

        IEnumerable<ReservationGroup> SearchAllReservations(string currentUserId, string query, ReservationSearchTypeEnum? searchType, DateTime startDate, DateTime endDate);
    }
}
