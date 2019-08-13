using PoolReservation.Database.Entity;
using PoolReservation.Database.Entity.Model.Pictures;
using PoolReservation.Database.Entity.SharedObjects.Repository.EntityFramework6;
using PoolReservation.Infrastructure.Storage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PoolReservation.Infrastructure.Images
{
    public class GalleryManager
    {
        public readonly string DEFAULT_FILE_NAME = ".jpeg";
        public readonly string[] ALLOWED_FILE_NAMES = { ".png", ".jpeg", ".jpg", ".gif", ".bmp" };
        public readonly Size[] DEFAULT_THUMBNAIL_SIZES = { new Size { Width = 0, Height = 128 } }; //Set to 0 if you want a particular dimension to be relative.
        public readonly double[] DEFAULT_THUMNAIL_DOWNSIZES = { 0.5, 0.25 };
        public readonly int DEFAULT_FILE_URL_ID = 2;
        public GalleryManager()
        {

        }

        public async Task<Guid?> UploadImage(byte[] pictureData, string ownerId, ImageFormat format = null)
        {
            if (pictureData == null)
            {
                throw new Exception("pictureData cannot be null");
            }

            if (pictureData.Length == 0)
            {
                throw new Exception("The pictureData must not be empty");
            }

            var theImage = ImageFactory.CreateImageFromByteArray(pictureData);

            if (theImage.Height == 0 || theImage.Width == 0)
            {
                throw new Exception("Invalid picture size.");
            }

            var picturesToUpload = GenerateThumbnails(theImage);
            picturesToUpload.Add(theImage);

            var convertedPicturesToUpload = picturesToUpload.Select(x =>
            {
                var daByteArray = ImageFactory.CreateByteArrayFromImage(x, format); return new TemporaryPictureObject
                {
                    Data = daByteArray,
                    FileName = ImageFactory.GenerateFileName(format),
                    Height = x.Height,
                    Width = x.Width,
                    Size = daByteArray.Length,
                    FileUrlId = DEFAULT_FILE_URL_ID

                };
            }).ToList();

            var uploadToBlobTask = UploadToBlobStorage(convertedPicturesToUpload);

            Guid? thePictureId = null;

            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var thePicture = unitOfWork.Pictures.UploadPicture(ownerId, convertedPicturesToUpload);

                    await uploadToBlobTask;

                    unitOfWork.Complete();

                    thePictureId = thePicture.Id;
                    
                }
            }
            catch (Exception e)
            {
                await TryToDeleteItemsFromAzure(convertedPicturesToUpload.Select(x => x.FileName).ToArray());
                throw e;
            }

            return thePictureId;
        }

        public async Task TryToDeleteItemsFromAzure(params string[] filenames)
        {
            var azureManager = new AzureStorageManager();

            foreach (var filename in filenames)
            {
                try
                {
                    await azureManager.DeleteItemFromStorageIfExists(filename);
                }
                catch (Exception)
                {

                }
            }
        }

        public async Task UploadToBlobStorage(IEnumerable<TemporaryPictureObject> picture)
        {
            var azureManager = new AzureStorageManager();

            await Task.WhenAll(picture.Select(pic => azureManager.UploadFile(pic.FileName, pic.Data)).ToList());

        }



        public List<Image> GenerateThumbnails(Image theImage)
        {
            var thumbnails = new List<Image>();

            foreach (var size in DEFAULT_THUMBNAIL_SIZES)
            {
                var widthToUse = 0;
                var heightToUse = 0;

                if (size.Width == 0 && size.Height == 0)
                {
                    throw new Exception("Illegal picture size.");
                }
                else if (size.Width == 0)
                {
                    if (theImage.Height < size.Height)
                    {
                        continue;
                    }

                    heightToUse = size.Height;
                    var ratio = ((double)size.Height)/((double)theImage.Height);
                    widthToUse = (int)Math.Round((theImage.Width * ratio));
                }
                else if (size.Height == 0)
                {
                    if (theImage.Width < size.Width)
                    {
                        continue;
                    }

                    widthToUse = size.Width;
                    var ratio = ((double)size.Width)/((double)theImage.Width);
                    heightToUse = (int)Math.Round((theImage.Height * ratio));
                }
                else
                {
                    if (theImage.Width < size.Width || theImage.Height < size.Height)
                    {
                        continue;
                    }

                    widthToUse = size.Width;
                    heightToUse = size.Height;
                }

                if(widthToUse == 0 || heightToUse == 0)
                {
                    continue;
                }

                var newImage = ImageFactory.ResizeImage(theImage, widthToUse, heightToUse) as Image;

                if (newImage == null)
                {
                    continue;
                }

                thumbnails.Add(newImage);
            }

            foreach (var resDrop in DEFAULT_THUMNAIL_DOWNSIZES)
            {
                var widthToUse = (int)(theImage.Width * resDrop);
                var heightToUse = (int)(theImage.Height * resDrop);

                if (widthToUse == 0 || heightToUse == 0)
                {
                    continue;
                }

                var newImage = ImageFactory.ResizeImage(theImage, widthToUse, heightToUse) as Image;

                if (newImage == null)
                {
                    continue;
                }

                thumbnails.Add(newImage);
            }

            return thumbnails;
        }
    }
}