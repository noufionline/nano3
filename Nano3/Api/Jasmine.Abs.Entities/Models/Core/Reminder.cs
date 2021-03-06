﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class Reminder : TrackableEntityBase
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string EntityName { get; set; }
        public int? EntityId { get; set; }
        public int? TaskId { get; set; }
        public int? LogId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ReminderDate { get; set; }
        [Required]
        [StringLength(250)]
        public string ReminderNote { get; set; }
        public bool IsPrivate { get; set; }
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
    }
}
