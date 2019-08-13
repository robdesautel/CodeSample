using PoolReservation.Database.Entity.SharedObjects.Repository.Base.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoolReservation.Database.Entity.Model.Pictures;
using System.Data.Entity;

namespace PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6.Repositories
{
    public class PictureRepository : Repository, IPictureRepository
    {
        public PictureRepository(PoolReservationEntities context, UnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }

        public bool CanUserAddIcon(string currentUserId)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(currentUserId);

            return user?.SitePermissions?.IconPermissions?.Add ?? false;
        }

        public bool CanUserDeleteIcon(string currentUserId, int iconId)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(currentUserId);

            return user?.SitePermissions?.IconPermissions?.Delete ?? false;
        }

        public bool CanUserEditIcon(string currentUserId, int iconId)
        {
            var user = this.unitOfWork.Permissions.GetUserByIdWithPermissions(currentUserId);

            return user?.SitePermissions?.IconPermissions?.Edit ?? false;
        }

        public Icons CreateIconLink(string currentUserId, Guid PictureId)
        {
            var auth = this.CanUserAddIcon(currentUserId);

            if(auth == false)
            {
                throw new UnauthorizedAccessException();
            }

            var picture = this.GetPicture(currentUserId, PictureId);

            if(picture == null)
            {
                throw new Exception();
            }

            var newIconLink = new Icons
            {
                PictureId = picture.Id
            };

            this.dbContext.Icons.Add(newIconLink);

            return newIconLink;

        }

        public void DeleteIconLink(string currentUserId, int iconId)
        {
            var auth = this.CanUserDeleteIcon(currentUserId, iconId);

            if (auth == false)
            {
                throw new UnauthorizedAccessException();
            }

            var link = this.dbContext.Icons.FirstOrDefault(x => x.Id == iconId);

            if (link == null)
            {
                throw new Exception();
            }

            link.IsDeleted = true;
            link.DeletedDate = DateTime.UtcNow;
        }

        public Icons EditIconLink(string currentUserId, int iconId, Guid newPictureId)
        {
            var auth = this.CanUserEditIcon(currentUserId, iconId);

            if (auth == false)
            {
                throw new UnauthorizedAccessException();
            }

            var picture = this.GetPicture(currentUserId, newPictureId);

            if (picture == null)
            {
                throw new Exception();
            }

            var icon = this.dbContext.Icons.FirstOrDefault(x => x.Id == iconId);

            if(icon == null)
            {
                throw new Exception();
            }

            icon.PictureId = picture.Id;

            return icon;
        }

        public IEnumerable<Icons> GetAllIcons(string currentUserId)
        {
            return this.dbContext.Icons
                .Include(x => x.Pictures)
                .Include(x => x.Pictures.PictureResolutions)
                .Include(x => x.Pictures.PictureResolutions.Select(y => y.PictureUrls))
                .Where(x => x.IsDeleted == false && x.Pictures.IsDeleted == false);
        }

        public Icons GetIconLink(string currentUserId, int iconId)
        {
            return this.dbContext.Icons
                .Include(x => x.Pictures)
                .Include(x => x.Pictures.PictureResolutions)
                .Include(x => x.Pictures.PictureResolutions.Select(y => y.PictureUrls))
                .FirstOrDefault(x => x.Id == iconId);
        }

        public Pictures GetPicture(string currentUserId, Guid pictureId)
        {
            return this.dbContext.Pictures.Include(x => x.PictureResolutions.Select(y => y.PictureUrls)).FirstOrDefault(x => x.Id == pictureId);
        }

        public Pictures UploadPicture(string currentUserId, IEnumerable<TemporaryPictureObject> pictureData)
        {
            var user = this.unitOfWork.Users.GetUserById(currentUserId, currentUserId);

            if(user == null)
            {
                throw new Exception("Unable to find user.");
            }

            var picture = new Pictures();
           
            picture.OwnerId = user.Id;
            picture.DateUploaded = DateTime.UtcNow;
            

            foreach(var pic in pictureData)
            {
                var picToAdd = new PictureResolutions
                {
                    FileName = pic.FileName,
                    FileUrlId = pic.FileUrlId,
                    Size = pic.Size,
                    Width = pic.Width,
                    Height = pic.Height,
                };

                picture.PictureResolutions.Add(picToAdd);
            }


            this.dbContext.Pictures.Add(picture);

            return picture;
        }
    }
}
