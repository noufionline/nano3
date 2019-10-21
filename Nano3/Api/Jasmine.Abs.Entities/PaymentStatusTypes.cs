using System.ComponentModel.DataAnnotations;

namespace Jasmine.Abs.Entities
{
    public enum PaymentStatusTypes:int
    {
        [Display(Name = "In Hand")]
        InHand = 1,
        Submit = 2,
        Bounced = 3,
        [Display(Name = "On Hold")]
        OnHold = 4,
        Cleared = 5,
        [Display(Name = "Re-Submit")]
        ReSubmit = 6,
        Replaced = 7,
    }

    public enum CommercialInvoiceStatusTypes : int
    {
        Draft=0,
        WithClient=1,
        Signed=2,
        WithBank=3,
        NoConfirmed=4,
        Confirmed=5,
        Released=6,
        Rejected=7
    }
}