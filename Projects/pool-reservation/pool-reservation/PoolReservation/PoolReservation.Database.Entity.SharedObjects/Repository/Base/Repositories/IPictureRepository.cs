using PoolReservation.Database.Entity.Model.Pictures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolReservation.Database.Entity.SharedObjects.Repository.Base.Repositories
{
    public interface IPictureRepository : IRepository
    {
        //bool CanUserAddPictures(string userId);
        //bool CanUserEditPictures(string userId, int pictureId);
        //bool CanUserViewPictures(string userId, int pictureId);
        //bool CanUserDeletePictures(string userId, int pictureId);

        Pictures UploadPicture(string currentUserId, IEnumerable<TemporaryPictureObject> pictureData);

        Icons CreateIconLink(string currentUserId, Guid PictureId);

        void DeleteIconLink(string currentUserId, int iconId);

        Icons EditIconLink(string currentUserId, int iconId, Guid newPictureId);

        Icons GetIconLink(string currentUserId, int iconId);

        IEnumerable<Icons> GetAllIcons(string currentUserId);

        bool CanUserAddIcon(string currentUserId);
        bool CanUserEditIcon(string currentUserId, int iconId);
        bool CanUserDeleteIcon(string currentUserId, int iconId);

        Pictures GetPicture(string currentUserId, Guid pictureId);
    }
}
