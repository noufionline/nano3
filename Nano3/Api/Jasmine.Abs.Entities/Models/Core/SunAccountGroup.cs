using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class SunAccountGroup
    {
        public SunAccountGroup()
        {
            SunAccountCodeGroupMappings = new HashSet<SunAccountCodeGroupMapping>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        public int? ProjectId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [Required]
        [StringLength(150)]
        public string CreatedUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }
        [StringLength(150)]
        public string ModifiedUser { get; set; }
        public byte[] RowVersion { get; set; }

        [ForeignKey("ProjectId")]
        [InverseProperty("SunAccountGroups")]
        public virtual PartnerProject Project { get; set; }
        [InverseProperty("Group")]
        public virtual ICollection<SunAccountCodeGroupMapping> SunAccountCodeGroupMappings { get; set; }
    }
}
