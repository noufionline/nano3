using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class SunSystemDataFile
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        public long Length { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreationTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime LastAccessTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime LastWriteTime { get; set; }
        [Required]
        [StringLength(50)]
        public string ImportedUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ImportedTime { get; set; }
        public Guid AttachmentId { get; set; }
    }
}
