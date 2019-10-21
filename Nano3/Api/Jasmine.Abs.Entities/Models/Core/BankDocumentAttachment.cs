using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class BankDocumentAttachment
    {
        [Key]
        public int Id { get; set; }
        public Guid? AttachmentFileId { get; set; }
        public int AttachmentTypeId { get; set; }
        [StringLength(50)]
        public string AttachmentVersion { get; set; }
        public int BankDocumentId { get; set; }
        public bool Expired { get; set; }
        public byte[] RowVersion { get; set; }

        [ForeignKey("AttachmentTypeId")]
        [InverseProperty("BankDocumentAttachments")]
        public virtual BankDocumentAttachmentType AttachmentType { get; set; }
        [ForeignKey("BankDocumentId")]
        [InverseProperty("BankDocumentAttachments")]
        public virtual AccountReceivable BankDocument { get; set; }
    }
}
