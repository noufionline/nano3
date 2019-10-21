using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
[Table("SunAccountCodeGroupMapping")]
    public partial class SunAccountCodeGroupMapping
    {
        [Key]
        [StringLength(15)]
        public string AccountCode { get; set; }
        [Key]
        public int GroupId { get; set; }

        [ForeignKey("AccountCode")]
        [InverseProperty("SunAccountCodeGroupMappings")]
        public virtual CustomersFromSunSystem AccountCodeNavigation { get; set; }
        [ForeignKey("GroupId")]
        [InverseProperty("SunAccountCodeGroupMappings")]
        public virtual SunAccountGroup Group { get; set; }
    }
}
