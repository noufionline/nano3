using System;
using System.Globalization;
using System.Windows.Data;

//using Jasmine.Core.Security;

namespace Jasmine.Core.Converters
{
    public class ClaimsToBooleanConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            //if (ClaimsPrincipal.Current is JPrincipal principal && parameter!=null)
            //{
            //   return principal.HasClaim("Permission", parameter.ToString());
            //}
            false;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}