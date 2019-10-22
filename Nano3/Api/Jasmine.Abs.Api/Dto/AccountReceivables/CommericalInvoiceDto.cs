using Jasmine.Abs.Entities;
using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Api.Dto.AccountReceivables
{
    public class CommercialInvoiceDto : TrackableEntityBase, IEntity
    {
        public int Id { get; set; }
        public int LcDocumentId { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal Amount { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime StatusDate { get; set; }
        public CommercialInvoiceStatusTypes CommercialInvoiceStatus { get; set; }
        public string StatusRemarks { get; set; }
        public string Remarks { get; set; }
        public Guid? DraftCopyId { get; set; }
        public Guid? SignedCopyId { get; set; }
        public string DraftCopyVersion { get; set; }
        public string SignedCopyVersion { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }

        public List<CommercialInvoiceTransactionHistoryDto> CommercialInvoiceTransactionHistories { get; set; }
        public List<CommercialInvoiceAttachmentDto> CommercialInvoiceAttachments { get; set; }
    }


    public class CommercialInvoiceLineForPrintDto
    {
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal Amount { get; set; }
        public DateTime? DueDate { get; set; }
        public CommercialInvoiceStatusTypes CommercialInvoiceStatus { get; set; }
        public DateTime StatusDate { get; set; }
        public string StatusRemarks { get; set; }
        public DateTime? DateOfSignature { get; set; }
    }

    public class CommercialInvoiceLineDto : TrackableEntityBase, IEntity
    {
        public int Id { get; set; }
        public int LcDocumentId { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal Amount { get; set; }
        public DateTime? DueDate { get; set; }
        public CommercialInvoiceStatusTypes CommercialInvoiceStatus { get; set; }
        public DateTime StatusDate { get; set; }
        public string StatusRemarks { get; set; }
        public Guid? FileId { get; set; }
        public string Remarks { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }

        public List<CommercialInvoiceTransactionHistoryDto> CommercialInvoiceTransactionHistories { get; set; }
    }
}