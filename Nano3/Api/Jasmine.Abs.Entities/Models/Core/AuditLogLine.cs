using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class AuditLogLine
    {
        [Key]
        public int Id { get; set; }
        public int AuditLogId { get; set; }
        public int? PrimaryKey { get; set; }
        [StringLength(250)]
        public string EntityName { get; set; }
        [StringLength(250)]
        public string PropertyName { get; set; }
        [StringLength(250)]
        public string PropertyType { get; set; }
        public string Before { get; set; }
        public string After { get; set; }
        public int? State { get; set; }

        [ForeignKey("AuditLogId")]
        [InverseProperty("AuditLogLines")]
        public virtual AuditLog AuditLog { get; set; }
    }
}
