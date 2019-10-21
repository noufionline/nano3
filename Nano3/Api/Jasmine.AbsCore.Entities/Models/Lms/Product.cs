using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Lms
{
    public partial class Product:TrackableEntityBase
    {
        public Product()
        {
            InboundItems = new HashSet<InboundItem>();
            OutboundItems = new HashSet<OutboundItem>();
        }

        public int ProductId { get; set; }
        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }
        public int? UnitId { get; set; }
        [Column("ProductName_ABS")]
        [StringLength(50)]
        public string ProductNameAbs { get; set; }

        [ForeignKey("UnitId")]
        [InverseProperty("Products")]
        public ProductUnit Unit { get; set; }
        [InverseProperty("Product")]
        public ICollection<InboundItem> InboundItems { get; set; }
        [InverseProperty("Product")]
        public ICollection<OutboundItem> OutboundItems { get; set; }
    }
}
