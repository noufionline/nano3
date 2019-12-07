using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Jasmine.Core.Converters
{
    public class ByteBitmapSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            byte[] img = value as byte[];
            var converter = new ImageSourceConverter();
            var bmpSrc = (BitmapSource)converter.ConvertFrom(img);
            bmpSrc.Freeze();
            return bmpSrc;            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var imageSource = value as ImageSource;
            if (imageSource != null)
            {
                using (var stream = new MemoryStream())
                {
                    var encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create((BitmapSource)imageSource));
                    encoder.Save(stream);
                    return stream.ToArray();
                }
            }
            return Binding.DoNothing;
        }
    }
}