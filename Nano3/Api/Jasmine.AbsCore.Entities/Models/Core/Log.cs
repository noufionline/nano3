using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
    public partial class Log
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string EntityName { get; set; }
        public int? EntityId { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime LogDate { get; set; }
        [Required]
        [StringLength(250)]
        public string LogNote { get; set; }
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
    }
}
