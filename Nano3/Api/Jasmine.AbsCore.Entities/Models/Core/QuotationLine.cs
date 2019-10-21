using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
    public partial class QuotationLine
    {
        [Key]
        public int Id { get; set; }
        public int SlNo { get; set; }
        public int QuotationId { get; set; }
        public int? ProductCategoryId { get; set; }
        [Required]
        [StringLength(500)]
        public string Description { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal Qty { get; set; }
        [StringLength(50)]
        public string Unit { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPrice { get; set; }
        public bool IsRateOnly { get; set; }
        public byte[] RowVersion { get; set; }

        [ForeignKey("ProductCategoryId")]
        [InverseProperty("QuotationLines")]
        public virtual ProductCategory ProductCategory { get; set; }
        [ForeignKey("QuotationId")]
        [InverseProperty("QuotationLines")]
        public virtual Quotation Quotation { get; set; }
    }
}
