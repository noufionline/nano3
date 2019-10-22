using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class ProductCategory : TrackableEntityBase
    {
        public ProductCategory()
        {
            QuotationLines = new HashSet<QuotationLine>();
            QuotationMiscProducts = new HashSet<QuotationMiscProduct>();
            SalesTermsByProductCategories = new HashSet<SalesTermsByProductCategory>();
            UnitsByProductCategories = new HashSet<UnitsByProductCategory>();
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
        public byte[] RowVersion { get; set; }
        public bool IsSteel { get; set; }

        [InverseProperty("ProductCategory")]
        public virtual ICollection<QuotationLine> QuotationLines { get; set; }
        [InverseProperty("ProductCategory")]
        public virtual ICollection<QuotationMiscProduct> QuotationMiscProducts { get; set; }
        [InverseProperty("ProductCategory")]
        public virtual ICollection<SalesTermsByProductCategory> SalesTermsByProductCategories { get; set; }
        [InverseProperty("ProductCategory")]
        public virtual ICollection<UnitsByProductCategory> UnitsByProductCategories { get; set; }
    }
}
