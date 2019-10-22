using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class AccountReceivable : TrackableEntityBase
    {
        public AccountReceivable()
        {
            AllocatedInvoices = new HashSet<AllocatedInvoice>();
            BankDocumentAttachments = new HashSet<BankDocumentAttachment>();
            BankDocumentTransactionHistories = new HashSet<BankDocumentTransactionHistory>();
            PaymentReceiptVouchers = new HashSet<PaymentReceiptVoucher>();
        }

        [Key]
        public int Id { get; set; }
        public int PartnerId { get; set; }
        public int? ProjectId { get; set; }
        [StringLength(50)]
        public string SunAccountCode { get; set; }
        public int? BankId { get; set; }
        [StringLength(25)]
        public string DocumentNo { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? DocumentDate { get; set; }
        public AccountReceivableTypes DocumentType { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? NextActionDate { get; set; }
        [StringLength(50)]
        public string CollectionVoucherNo { get; set; }
        [StringLength(250)]
        public string Remarks { get; set; }
        [Required]
        [StringLength(150)]
        public string CreatedUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [StringLength(150)]
        public string ModifiedUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public int CollectionVoucherId { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? CollectionVoucherDate { get; set; }
        public int? CollectedById { get; set; }
        public PaymentStatusTypes PaymentStatusId { get; set; }
        public int CompanyId { get; set; }
        public bool CanSubmit { get; set; }

        [ForeignKey("BankId")]
        [InverseProperty("AccountReceivables")]
        public virtual Bank Bank { get; set; }
        [ForeignKey("CollectedById")]
        [InverseProperty("AccountReceivables")]
        public virtual AccountReceivableCollector CollectedBy { get; set; }
        [ForeignKey("CompanyId")]
        [InverseProperty("AccountReceivables")]
        public virtual Company Company { get; set; }
        [ForeignKey("PartnerId")]
        [InverseProperty("AccountReceivables")]
        public virtual Partner Partner { get; set; }
        [ForeignKey("ProjectId")]
        [InverseProperty("AccountReceivables")]
        public virtual PartnerProject Project { get; set; }
        [InverseProperty("BankDocument")]
        public virtual ICollection<AllocatedInvoice> AllocatedInvoices { get; set; }
        [InverseProperty("BankDocument")]
        public virtual ICollection<BankDocumentAttachment> BankDocumentAttachments { get; set; }
        [InverseProperty("BankDocument")]
        public virtual ICollection<BankDocumentTransactionHistory> BankDocumentTransactionHistories { get; set; }
        [InverseProperty("AccountReceivable")]
        public virtual ICollection<PaymentReceiptVoucher> PaymentReceiptVouchers { get; set; }
    }
}
