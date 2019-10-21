using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Lms
{
    public partial class OutboundItem:TrackableEntityBase
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public int ProductId { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal Qty { get; set; }

        [ForeignKey("DocumentId")]
        [InverseProperty("OutboundItems")]
        public OutboundDocument Document { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("OutboundItems")]
        public Product Product { get; set; }
    }
}
