using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class SalesOrderAttachment
    {
        [Key]
        public int Id { get; set; }
        public Guid? AttachmentFileId { get; set; }
        public string AttachmentPath { get; set; }
        [Required]
        public byte[] RowVersion { get; set; }
        public int SalesOrderId { get; set; }
        [Required]
        [StringLength(50)]
        public string AttachmentType { get; set; }
        [StringLength(50)]
        public string AttachmentVersion { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? IssuedDate { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? ExpiryDate { get; set; }
        public bool Expired { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? FollowUpDate { get; set; }
        public string FollowUpNote { get; set; }
        public string FollowUpHistory { get; set; }

        [ForeignKey("SalesOrderId")]
        [InverseProperty("SalesOrderAttachments")]
        public virtual SalesOrder SalesOrder { get; set; }
    }
}
