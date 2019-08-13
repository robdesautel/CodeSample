using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.SharedObjects.Repository.Base.Repositories
{
    public interface IUserRepository : IRepository
    {
        bool CanUserViewOtherUser(string currentUserId, string userIdToView);

        bool CanUserEditOtherUser(string currentUserId, string userIdToView);

        bool CanUserMinimalSearchOtherUsers(string currentUserId);

        AspNetUsers GetUserById(string currentUserId, string userIdToGet);

        AspNetUsers EditUser(string userId, string phonenumber, string firstname, string lastname);

        AspNetUsers GetUserByIdForLogin(string currentUserId, string userIdToGet);

        bool IsUserDeveloperAdmin(string id);

        AspNetUsers GetUserByEmail(string currentUserId, string emailToSearchBy);

        IEnumerable<AspNetUsers> SearchUserByIdOrEmailMINIMAL(string userId, string query);

        void SetProfilePictureADMIN(string userIdToSet, Guid pictureId);

        void DeleteSocialLinkingsForProviderSYSTEM(string userId, string provider);

    }
}
