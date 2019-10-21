using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Lms
{
    public partial class ProductUnit:TrackableEntityBase
    {
        public ProductUnit()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        public int UnitId { get; set; }
        [Required]
        [StringLength(50)]
        public string Unit { get; set; }

        [InverseProperty("Unit")]
        public ICollection<Product> Products { get; set; }
    }
}
