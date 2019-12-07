using FluentValidation;
using System;

namespace Jasmine.Core.Dialogs
{
    //public class DateRangeValidator : AbstractValidator<DateRange>
    //{
    //    public DateRangeValidator()
    //    {
    //        RuleFor(x => x.FromDate).NotNull().WithMessage("From Date is required");
    //        RuleFor(x => x.ToDate).NotNull().WithMessage("To Date is required");
    //        RuleFor(x => x.ToDate).Must(BeValid).WithMessage("Invalid To Date");
    //    }

    //    private bool BeValid(DateRange dateRange, DateTime? toDate) => toDate.GetValueOrDefault() >= dateRange.FromDate.GetValueOrDefault();
    //}
}