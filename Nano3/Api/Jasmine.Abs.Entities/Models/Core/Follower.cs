using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class Follower
    {
        [Key]
        public int Id { get; set; }
        public int TaskId { get; set; }
        [Required]
        [StringLength(150)]
        public string FollowingUser { get; set; }
        public byte[] RowVersion { get; set; }

        [ForeignKey("TaskId")]
        [InverseProperty("Followers")]
        public virtual UserTask Task { get; set; }
    }
}
