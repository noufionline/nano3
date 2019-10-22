using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jasmine.Abs.Api.Dto.AccountReceivables
{
    public class LcDocumentDetailDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string SunAccountCode { get; set; }
        public string ClientBankName { get; set; }
        public string ClientLcNo { get; set; }
        public string OurLcNo { get; set; }
        public string OurBankName { get; set; }
        public DateTime OpeningDate { get; set; }
        public decimal TotalLcValue { get; set; }
        public decimal AmountUtilized { get; set; }
        public decimal BalanceAmount => TotalLcValue - AmountUtilized;

    }
}
