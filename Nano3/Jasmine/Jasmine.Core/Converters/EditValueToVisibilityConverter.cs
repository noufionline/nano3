using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

//using Jasmine.Core.Security;

namespace Jasmine.Core.Converters
{
    public class EditValueToVisibilityConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {            
            if(values[2] is bool) return Visibility.Hidden;
            object editValue = values[0];
            if (editValue != null && values[1] is bool mouseOver && mouseOver)
                return Visibility.Visible;
            return Visibility.Hidden;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}