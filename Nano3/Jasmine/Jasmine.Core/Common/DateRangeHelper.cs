using System;

namespace Jasmine.Core.Common
{
    public class DateRangeHelper
    {
        public DateRangeHelper(DateTime fromDate, DateTime toDate)
        {
            if (fromDate == toDate)
            {
                _displayString = $"Date: {fromDate: dd - MMM - yyyy}";
            }
            else if (fromDate.Date.Day == 1 &&
                     fromDate.Year == toDate.Year &&
                     fromDate.Month == toDate.Month &&
                     DateTime.DaysInMonth(toDate.Year, toDate.Month) == toDate.Day)
            {
                _displayString = $"Month: {fromDate:MMM-yyyy}";
            }
            else if (fromDate.Date.Day == 1 &&
                     fromDate.Year == toDate.Year &&
                     fromDate.Month == 1 && toDate.Month == 12 &&
                     DateTime.DaysInMonth(toDate.Year, toDate.Month) == toDate.Day)
            {
                _displayString = $"Year: {fromDate:yyyy}";
            }
            else
            {
                if (fromDate < toDate)
                {
                    _displayString = $"Between {fromDate:dd-MMM-yyyy} and {toDate:dd-MMM-yyyy}";
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }


        }

        private readonly string _displayString;

        public override string ToString() => _displayString;
    }
}