using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Lms
{
    public partial class Supplier:TrackableEntityBase
    {
        public Supplier()
        {
            InboundDocuments = new HashSet<InboundDocument>();
        }

        public int SupplierId { get; set; }
        [Required]
        [StringLength(50)]
        public string SupplierName { get; set; }
        public int? DivisionId { get; set; }

        [InverseProperty("Supplier")]
        public ICollection<InboundDocument> InboundDocuments { get; set; }
    }
}
