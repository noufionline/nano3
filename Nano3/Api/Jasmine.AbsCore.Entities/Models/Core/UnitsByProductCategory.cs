using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
[Table("UnitsByProductCategory")]
    public partial class UnitsByProductCategory
    {
        [Key]
        public int ProductCategoryId { get; set; }
        [Key]
        public int UnitId { get; set; }
        public byte[] RowVersion { get; set; }

        [ForeignKey("ProductCategoryId")]
        [InverseProperty("UnitsByProductCategories")]
        public virtual ProductCategory ProductCategory { get; set; }
        [ForeignKey("UnitId")]
        [InverseProperty("UnitsByProductCategories")]
        public virtual ProductUnit Unit { get; set; }
    }
}
