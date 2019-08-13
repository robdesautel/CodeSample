using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace PoolReservation.Infrastructure.Images
{
    public static class ImageFactory
    {
        private static readonly ImageFormat DEFAULT_FORMAT = ImageFormat.Jpeg;

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            var graphics = Graphics.FromImage(destImage);


            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using (var wrapMode = new ImageAttributes())
            {
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }


            return destImage;
        }

        public static Image CreateImageFromByteArray(byte[] data)
        {

            var stream = new MemoryStream(data);
            return Image.FromStream(stream);
        }

        public static byte[] CreateByteArrayFromImage(Image data)
        {
            return CreateByteArrayFromImage(data, DEFAULT_FORMAT);
        }

        public static byte[] CreateByteArrayFromImage(Image data, ImageFormat format)
        {
            MemoryStream ms = new MemoryStream();
            data.Save(ms, format ?? DEFAULT_FORMAT);
            return ms.ToArray();
        }

        public static string GenerateFileName(ImageFormat format = null)
        {
            return $"{Guid.NewGuid().ToString()}.{(format ?? DEFAULT_FORMAT).ToString()}".Trim().ToLower();
        }

        public static byte[] ConvertBase64ToArray(string data)
        {

            var indexOfSplit = data.IndexOf("base64,");

            if (indexOfSplit >= 0)
            {
                indexOfSplit += "base64,".Length;
                data = data.Substring(indexOfSplit);
            }

            return Convert.FromBase64String(data);
        }
    }
}