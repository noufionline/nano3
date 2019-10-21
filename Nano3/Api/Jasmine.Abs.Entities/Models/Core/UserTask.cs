using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class UserTask
    {
        public UserTask()
        {
            Followers = new HashSet<Follower>();
            TaskAttachments = new HashSet<TaskAttachment>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string EntityName { get; set; }
        public int? EntityId { get; set; }
        [Required]
        [StringLength(250)]
        public string Subject { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? TaskDate { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? TaskEndDate { get; set; }
        [Column(TypeName = "text")]
        public string TaskNote { get; set; }
        [StringLength(500)]
        public string AssignedToUser { get; set; }
        [Required]
        [StringLength(50)]
        public string Status { get; set; }
        [StringLength(50)]
        public string Priority { get; set; }
        [StringLength(50)]
        public string FollowUp { get; set; }
        [StringLength(50)]
        public string Category { get; set; }
        public bool IsPrivate { get; set; }
        public bool HasReminder { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ReminderDate { get; set; }
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

        [InverseProperty("Task")]
        public virtual ICollection<Follower> Followers { get; set; }
        [InverseProperty("Task")]
        public virtual ICollection<TaskAttachment> TaskAttachments { get; set; }
    }
}
