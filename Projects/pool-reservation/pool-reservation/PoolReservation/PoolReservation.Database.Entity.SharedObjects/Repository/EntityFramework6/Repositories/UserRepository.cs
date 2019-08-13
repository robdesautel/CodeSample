using PoolReservation.Database.Entity.Model;
using PoolReservation.Database.Entity.SharedObjects.Repository.Base.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6.Repositories
{
    public class UserRepository : Repository, IUserRepository
    {
        public UserRepository(PoolReservationEntities context, UnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }

        public bool CanUserEditOtherUser(string currentUserId, string userIdToView)
        {
            if(currentUserId == userIdToView)
            {
                return true;
            }

            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(currentUserId);

            return user?.SitePermissions?.OtherUsersPermissions?.Edit ?? false;
        }

        public bool CanUserViewOtherUser(string currentUserId, string userIdToView)
        {
            if (currentUserId == userIdToView)
            {
                return true;
            }

            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(currentUserId);

            return user?.SitePermissions?.OtherUsersPermissions?.View ?? false;
        }

        public AspNetUsers GetUserByEmail(string currentUserId, string email)
        {
            var canView = this.CanUserViewOtherUser(currentUserId, currentUserId);

            if(canView == false)
            {
                throw new UnauthorizedAccessException();
            }


            return this.dbContext.AspNetUsers.SingleOrDefault(x => x.Email == email);
        }

        public AspNetUsers GetUserById(string currentUserId, string userIdToGet)
        {
            var canView = this.CanUserViewOtherUser(currentUserId, userIdToGet);

            if (canView == false)
            {
                throw new UnauthorizedAccessException();
            }

            return this.dbContext.AspNetUsers.SingleOrDefault(x => x.Id == userIdToGet);
        }

        public AspNetUsers EditUser(string userId, string phonenumber, string firstname, string lastname)
        {
            var user = this.dbContext.AspNetUsers.FirstOrDefault(x => x.Id == userId);

            if(user == null)
            {
                throw new Exception();
            }

            if(phonenumber != null)
            {
                user.PhoneNumber = phonenumber;
            }

            if(firstname != null)
            {
                user.FirstName = firstname;
            }

            if(lastname != null)
            {
                user.LastName = lastname;
            }

            return user;

        }

        public AspNetUsers GetUserByIdForLogin(string currentUserId, string userIdToGet)
        {
            var canView = this.CanUserViewOtherUser(currentUserId, userIdToGet);

            if (canView == false)
            {
                throw new UnauthorizedAccessException();
            }

            return this.dbContext.AspNetUsers
                .Include(x => x.SitePermissions)
                .Include(x => x.SitePermissions.HotelPermissions)
                .Include(x => x.SitePermissions.IconPermissions)
                .Include(x => x.SitePermissions.OtherReservationsPermissions)
                .Include(x => x.SitePermissions.OtherUsersPermissions)
                .Include(x => x.SitePermissions.PersonalReservationsPermissions)
                .Include(x => x.SitePermissions.PricePermissions)
                .Include(x => x.ProfilePicture)
                .Include(x => x.ProfilePicture.PictureResolutions)
                .SingleOrDefault(x => x.Id == userIdToGet);
        }

        public bool IsUserDeveloperAdmin(string id)
        {
            var roleString = Convert.ToInt32(AspNetUserRolesEnum.DeveloperAdmin).ToString();

            return this.dbContext.AspNetUsers.Where(x => x.Id == id && x.AspNetRoles.Count(role => role.Id == roleString) != 0).Count() != 0;
        }

        public void SetProfilePictureADMIN(string userId, Guid pictureId)
        {
            var user = this.GetUserById(userId, userId);

            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            var picture = this.unitOfWork.Pictures.GetPicture(userId, pictureId);

            if (picture == null)
            {
                throw new Exception("Unable to find the picture");
            }

            user.ProfilePictureId = picture.Id;
        }

        public IEnumerable<AspNetUsers> SearchUserByIdOrEmailMINIMAL(string userId, string query)
        {
            var auth = this.CanUserMinimalSearchOtherUsers(userId);

            if(auth == false)
            {
                throw new UnauthorizedAccessException();
            }

            var minimalizedQuery = query?.Trim()?.ToLower();

            if(string.IsNullOrWhiteSpace(minimalizedQuery) || minimalizedQuery.Length <= 3)
            {
                throw new Exception();
            }

            return this.dbContext.AspNetUsers.Where(x => x.Id.Contains(minimalizedQuery) || x.Email.ToLower().Contains(minimalizedQuery));
        }

        public bool CanUserMinimalSearchOtherUsers(string currentUserId)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(currentUserId);

            return user?.SitePermissions?.OtherUsersPermissions?.View ?? false;
        }

        public void DeleteSocialLinkingsForProviderSYSTEM(string userId, string provider)
        {
            var socialLoginsToRemove = this.dbContext.AspNetUserLogins.Where(x => x.UserId == userId && x.LoginProvider == provider);

            this.dbContext.AspNetUserLogins.RemoveRange(socialLoginsToRemove);
        }
    }
}
