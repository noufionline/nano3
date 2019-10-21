using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
    public partial class PartnerGroupMapping
    {
        [Key]
        public int Id { get; set; }
        public int PartnerGroupId { get; set; }
        [StringLength(150)]
        public string Tag { get; set; }
        public int? PartnerId { get; set; }
        [Required]
        [StringLength(15)]
        public string AccountCode { get; set; }
        public byte[] RowVersion { get; set; }

        [ForeignKey("AccountCode")]
        [InverseProperty("PartnerGroupMappings")]
        public virtual CustomersFromSunSystem AccountCodeNavigation { get; set; }
        [ForeignKey("PartnerId")]
        [InverseProperty("PartnerGroupMappings")]
        public virtual Partner Partner { get; set; }
        [ForeignKey("PartnerGroupId")]
        [InverseProperty("PartnerGroupMappings")]
        public virtual PartnerGroup PartnerGroup { get; set; }
    }
}
