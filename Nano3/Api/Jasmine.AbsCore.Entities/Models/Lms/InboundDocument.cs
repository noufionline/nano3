using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Lms
{
    public partial class InboundDocument:TrackableEntityBase
    {
        public InboundDocument()
        {
            InboundItems = new HashSet<InboundItem>();
        }

        [Key]
        public int DocumentId { get; set; }
        [Required]
        [StringLength(50)]
        public string DocumentNo { get; set; }
        public int SupplierId { get; set; }
        public int InboundTripId { get; set; }
        public int DivisionId { get; set; }
        public int LocationId { get; set; }
        public int MovementTypeId { get; set; }
        [StringLength(50)]
        public string Remarks { get; set; }

        [ForeignKey("DivisionId")]
        [InverseProperty("InboundDocuments")]
        public Division Division { get; set; }
        [ForeignKey("InboundTripId")]
        [InverseProperty("InboundDocuments")]
        public InboundTripMaster InboundTrip { get; set; }
        [ForeignKey("LocationId")]
        [InverseProperty("InboundDocuments")]
        public Location Location { get; set; }
        [ForeignKey("MovementTypeId")]
        [InverseProperty("InboundDocuments")]
        public MovementType MovementType { get; set; }
        [ForeignKey("SupplierId")]
        [InverseProperty("InboundDocuments")]
        public Supplier Supplier { get; set; }
        [InverseProperty("Document")]
        public ICollection<InboundItem> InboundItems { get; set; }
    }
}
