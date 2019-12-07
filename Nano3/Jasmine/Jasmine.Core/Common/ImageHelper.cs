using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Jasmine.Core.Common
{
    public class ImageHelper
    {
        public static byte[] ImageToByte(Image img)
        {
            var converter = new ImageConverter();
            var imgByte = (byte[])converter.ConvertTo(img, typeof(byte[]));
            return imgByte;
        }

        public static byte[] ResizeImage(Image image, int width, int height, bool propotional)
        {
            var _width = width;
            var _height = height;

            if (propotional)
            {
                var imageHeight = image.Size.Height;
                var imageWidth = image.Size.Width;

                var smallestDimension = imageHeight < imageWidth ? imageHeight : imageWidth;
                if (smallestDimension == imageHeight)
                    _width = (imageWidth * _height) / imageHeight;
                else
                    _height = (imageHeight * _width) / imageWidth;
            }

            var destRect = new Rectangle(0, 0, _width, _height);
            var destImage = new Bitmap(_width, _height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.Low;
                graphics.SmoothingMode = SmoothingMode.HighSpeed;
                graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            if (propotional)
                destImage = ReCenter(destImage, width, height);

            var byteImage = ImageToByte(destImage);
            long quality = 90L;
            Bitmap compressedImage;
            while (byteImage.Length > 10000)
            {
                compressedImage = GetCompressedBitmap(destImage, quality);
                byteImage = ImageToByte(compressedImage);
                quality -= 10L;
            }

            return byteImage;
        }

        public static Bitmap ReCenter(Bitmap image, int width, int height)
        {
            var imageHeight = image.Size.Height;
            var imageWidth = image.Size.Width;
            var smallestDimension = imageHeight < imageWidth ? imageHeight : imageWidth;

            int startingXPoint = 0;
            int startingYPoint = 0;
            int horizontalEndPoint = 0;
            int verticalEndPoint = 0;
            if (smallestDimension == imageHeight)
            {
                var extraWidth = imageWidth - imageHeight;
                startingXPoint = extraWidth / 2;
                horizontalEndPoint = imageWidth - startingXPoint;
                verticalEndPoint = imageHeight;
            }
            else
            {
                var extraHeight = imageHeight - imageWidth;
                startingXPoint = 0;
                startingYPoint = extraHeight / 2;
                horizontalEndPoint = imageWidth;
                verticalEndPoint = imageHeight - startingYPoint;
            }

            var destRect = new Rectangle(0, 0, horizontalEndPoint, verticalEndPoint);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.Low;
                graphics.SmoothingMode = SmoothingMode.HighSpeed;
                graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;

                using (var wrapMode = new ImageAttributes())
                {
                    //wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, startingXPoint, startingYPoint, horizontalEndPoint, verticalEndPoint, GraphicsUnit.Pixel);
                }
            }

            return destImage;
        }

        public static Bitmap GetCompressedBitmap(Bitmap bmp, long quality)
        {
            //using (var mss = new MemoryStream())
            //{
            var mss = new MemoryStream();
            EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, quality);
            ImageCodecInfo imageCodec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(o => o.FormatID == ImageFormat.Jpeg.Guid);
            EncoderParameters parameters = new EncoderParameters(1);
            parameters.Param[0] = qualityParam;
            bmp.Save(mss, imageCodec, parameters);
            return (Bitmap)Image.FromStream(mss);
            //}
        }
    }
}
