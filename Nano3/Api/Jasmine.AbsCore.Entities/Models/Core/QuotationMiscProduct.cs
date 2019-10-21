using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
    public partial class QuotationMiscProduct
    {
        [Key]
        public int Id { get; set; }
        public int ProductCategoryId { get; set; }
        [Required]
        [StringLength(500)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Unit { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPrice { get; set; }

        [ForeignKey("ProductCategoryId")]
        [InverseProperty("QuotationMiscProducts")]
        public virtual ProductCategory ProductCategory { get; set; }
    }
}
