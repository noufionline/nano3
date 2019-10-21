using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
    public partial class CommercialInvoiceAttachment
    {
        [Key]
        public int Id { get; set; }
        public Guid? AttachmentFileId { get; set; }
        public int AttachmentTypeId { get; set; }
        [StringLength(50)]
        public string AttachmentVersion { get; set; }
        public int InvoiceId { get; set; }
        public bool Expired { get; set; }
        public byte[] RowVersion { get; set; }

        [ForeignKey("AttachmentTypeId")]
        [InverseProperty("CommercialInvoiceAttachments")]
        public virtual CommercialInvoiceAttachmentType AttachmentType { get; set; }
        [ForeignKey("InvoiceId")]
        [InverseProperty("CommercialInvoiceAttachments")]
        public virtual CommercialInvoice Invoice { get; set; }
    }
}
