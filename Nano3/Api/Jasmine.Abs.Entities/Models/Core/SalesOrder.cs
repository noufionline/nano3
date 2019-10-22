using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class SalesOrder : TrackableEntityBase
    {
        public SalesOrder()
        {
            SalesOrderAttachments = new HashSet<SalesOrderAttachment>();
            SalesOrderLines = new HashSet<SalesOrderLine>();
            SalesOrderQuotations = new HashSet<SalesOrderQuotation>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime OrderDate { get; set; }
        [Required]
        [StringLength(500)]
        public string CustomerRef { get; set; }
        [Required]
        [StringLength(500)]
        public string UniqueCustomerRef { get; set; }
        public int PartnerId { get; set; }
        public int? QuotationId { get; set; }
        public Guid? AttachmentFileId { get; set; }
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

        [ForeignKey("PartnerId")]
        [InverseProperty("SalesOrders")]
        public virtual Partner Partner { get; set; }
        [ForeignKey("QuotationId")]
        [InverseProperty("SalesOrders")]
        public virtual Quotation Quotation { get; set; }
        [InverseProperty("SalesOrder")]
        public virtual ICollection<SalesOrderAttachment> SalesOrderAttachments { get; set; }
        [InverseProperty("SalesOrder")]
        public virtual ICollection<SalesOrderLine> SalesOrderLines { get; set; }
        [InverseProperty("SalesOrder")]
        public virtual ICollection<SalesOrderQuotation> SalesOrderQuotations { get; set; }
    }
}
