using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
[Table("AutoMailInfo")]
    public partial class AutoMailInfo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string FromAddress { get; set; }
        [Required]
        [StringLength(250)]
        public string ToAddress { get; set; }
        [Required]
        [StringLength(250)]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public byte[] RowVersion { get; set; }
    }
}
