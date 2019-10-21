using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class QuotationAttachment
    {
        [Key]
        public int Id { get; set; }
        public string AttachmentPath { get; set; }
        public Guid? AttachmentFileId { get; set; }
        [Required]
        [StringLength(50)]
        public string AttachmentType { get; set; }
        [StringLength(50)]
        public string AttachmentVersion { get; set; }
        public bool Expired { get; set; }
        public int? QuotationId { get; set; }
        public byte[] RowVersion { get; set; }

        [ForeignKey("QuotationId")]
        [InverseProperty("QuotationAttachments")]
        public virtual Quotation Quotation { get; set; }
    }
}
