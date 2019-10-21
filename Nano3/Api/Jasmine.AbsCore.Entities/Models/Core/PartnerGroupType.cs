using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
    public partial class PartnerGroupType
    {
        public PartnerGroupType()
        {
            PartnerGroups = new HashSet<PartnerGroup>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [Required]
        [StringLength(150)]
        public string CreatedUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }
        [StringLength(150)]
        public string ModifiedUser { get; set; }
        [Required]
        public byte[] RowVersion { get; set; }

        [InverseProperty("PartnerGroupType")]
        public virtual ICollection<PartnerGroup> PartnerGroups { get; set; }
    }
}
