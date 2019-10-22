using Jasmine.Abs.Entities;
using System;


namespace Jasmine.Abs.Api.Dto.AccountReceivables
{
    public class CommercialInvoiceListDto : IEntity
    {
        public int Id { get; set; }
        public string LcDocumentNo { get; set; }
        public string CustomerName { get; set; }
        public string SunAccountCode { get; set; }
        public DateTime OpeningDate { get; set; }
        public string Bank { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal Amount { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime StatusDate { get; set; }
        public CommercialInvoiceStatusTypes CommercialInvoiceStatus { get; set; }
        public string StatusRemarks { get; set; }
        public Guid? FileId { get; set; }
        public string Remarks { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
    }

    
}