using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using DevExpress.Data.Async.Helpers;

namespace Jasmine.Core.Converters
{
    public class StringToTokenEditValueConverter : IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            string valueString = value.ToString();

            var list = new List<object>();

            string[] items = valueString.Split(',');

            foreach (string item in items)
            {
                list.Add(item);
            }

            return list;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is List<object> list && list.Count > 0)
            {
                //var str = new StringBuilder();
                //foreach (object item in list)
                //{
                //    str.Append($"{item},");
                //}

                //string valueString = str.ToString();
                //return valueString.Remove(valueString.Length - 1);
                var s = string.Join(",", list);
                return s;
            }

            return null;
        }
    }


    public class ODataInstantDataSourceRowConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ReadonlyThreadSafeProxyForObjectFromAnotherThread row)
                return row.OriginalRow;
            return null;
        }
    }
}