using System.ComponentModel.DataAnnotations;

namespace Jasmine.Abs.Entities
{
    public enum AccountReceivableTypes : int
    {
        Cheque = 1,
        [Display(Name = "Letter of Credit(LC)")]
        Lc = 2,
        Cash = 3,
        [Display(Name = "Bank Transfer")]
        BankTransfer = 4
    }
    

}