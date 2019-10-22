using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class TaskAttachment : TrackableEntityBase
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
        public int? TaskId { get; set; }
        public byte[] RowVersion { get; set; }

        [ForeignKey("TaskId")]
        [InverseProperty("TaskAttachments")]
        public virtual UserTask Task { get; set; }
    }
}
