using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class CommercialInvoice : TrackableEntityBase
    {
        public CommercialInvoice()
        {
            CommercialInvoiceAttachments = new HashSet<CommercialInvoiceAttachment>();
            CommercialInvoiceTransactionHistories = new HashSet<CommercialInvoiceTransactionHistory>();
        }

        [Key]
        public int Id { get; set; }
        public int LcDocumentId { get; set; }
        [Required]
        [StringLength(50)]
        public string InvoiceNo { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime InvoiceDate { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? DueDate { get; set; }
        public CommercialInvoiceStatusTypes CommercialInvoiceStatus { get; set; }
        public Guid? DraftCopyId { get; set; }
        public Guid? SignedCopyId { get; set; }
        [StringLength(50)]
        public string DraftCopyVersion { get; set; }
        [StringLength(50)]
        public string SignedCopyVersion { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime StatusDate { get; set; }
        [StringLength(250)]
        public string StatusRemarks { get; set; }
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

        [ForeignKey("LcDocumentId")]
        [InverseProperty("CommercialInvoices")]
        public virtual LcDocument LcDocument { get; set; }
        [InverseProperty("Invoice")]
        public virtual ICollection<CommercialInvoiceAttachment> CommercialInvoiceAttachments { get; set; }
        [InverseProperty("Invoice")]
        public virtual ICollection<CommercialInvoiceTransactionHistory> CommercialInvoiceTransactionHistories { get; set; }
    }
}
