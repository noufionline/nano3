using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class SalesOrderLine : TrackableEntityBase
    {
        [Key]
        public int Id { get; set; }
        public int SalesOrderId { get; set; }
        [Required]
        [StringLength(500)]
        public string Description { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal Qty { get; set; }
        [Required]
        [StringLength(50)]
        public string Unit { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPrice { get; set; }
        [Required]
        public byte[] RowVersion { get; set; }

        [ForeignKey("SalesOrderId")]
        [InverseProperty("SalesOrderLines")]
        public virtual SalesOrder SalesOrder { get; set; }
    }
}
