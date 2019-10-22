using Jasmine.Abs.Entities;
using System;


namespace Jasmine.Abs.Api.Dto.AccountReceivables
{
    public class LcDocumentListDto : IEntity
    {
        
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string ClientBankName { get; set; }
        public string ClientLcNo { get; set; }
        public string BankName { get; set; }
        public string Name { get; set; }
        public DateTime OpeningDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime NegotiationDate { get; set; }
        public decimal TotalLcValue { get; set; }
        public decimal AmountUtilized {get;set;}
        public decimal BalanceAmount => TotalLcValue - AmountUtilized;
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}