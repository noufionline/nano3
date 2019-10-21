using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class AuditLog
    {
        public AuditLog()
        {
            AuditLogLines = new HashSet<AuditLogLine>();
        }

        [Key]
        public int Id { get; set; }
        public int PrimaryKey { get; set; }
        [Required]
        [StringLength(250)]
        public string EntityName { get; set; }
        public string Before { get; set; }
        public string Changes { get; set; }
        public string After { get; set; }
        public int? State { get; set; }
        [Required]
        [StringLength(150)]
        public string RegUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime RegDate { get; set; }

        [InverseProperty("AuditLog")]
        public virtual ICollection<AuditLogLine> AuditLogLines { get; set; }
    }
}
