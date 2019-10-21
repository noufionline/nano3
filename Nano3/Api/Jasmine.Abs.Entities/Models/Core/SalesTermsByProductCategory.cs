using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
[Table("SalesTermsByProductCategory")]
    public partial class SalesTermsByProductCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(500)]
        public string Name { get; set; }
        public int ProductCategoryId { get; set; }

        [ForeignKey("ProductCategoryId")]
        [InverseProperty("SalesTermsByProductCategories")]
        public virtual ProductCategory ProductCategory { get; set; }
    }
}
