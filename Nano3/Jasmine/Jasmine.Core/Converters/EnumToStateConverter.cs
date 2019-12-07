using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows.Data;
using Humanizer;

namespace Jasmine.Core.Converters
{
    public class EnumToStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    var type = value.GetType();
                    var member = type.GetMember(value.ToString()).FirstOrDefault();
                    var displayName = member?
                        .GetCustomAttributes<DisplayAttribute>()
                        .FirstOrDefault();
                    if (displayName != null)
                    {
                        return displayName.Name;
                    }
                    var enumString = Enum.GetName(value.GetType(), value);
                    return enumString.Humanize(LetterCasing.Title);
                }

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        // No need to implement converting back on a one-way binding 
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}