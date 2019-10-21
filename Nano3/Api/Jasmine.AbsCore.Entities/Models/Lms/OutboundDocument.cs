using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Lms
{
    public partial class OutboundDocument:TrackableEntityBase
    {
        public OutboundDocument()
        {
            OutboundItems = new HashSet<OutboundItem>();
        }

        [Key]
        public int DocumentId { get; set; }
        [Column("DONo")]
        public int Dono { get; set; }
        public int CustomerId { get; set; }
        public int ProjectId { get; set; }
        public int OutboundTripId { get; set; }
        public int DivisionId { get; set; }
        public int LocationId { get; set; }
        public int? DeliveryTypeId { get; set; }
        public bool HasMillCertificate { get; set; }
        public int MovementTypeId { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("OutboundDocuments")]
        public Customer Customer { get; set; }
        [ForeignKey("DeliveryTypeId")]
        [InverseProperty("OutboundDocuments")]
        public DeliveryType DeliveryType { get; set; }
        [ForeignKey("DivisionId")]
        [InverseProperty("OutboundDocuments")]
        public Division Division { get; set; }
        [ForeignKey("LocationId")]
        [InverseProperty("OutboundDocuments")]
        public Location Location { get; set; }
        [ForeignKey("MovementTypeId")]
        [InverseProperty("OutboundDocuments")]
        public MovementType MovementType { get; set; }
        [ForeignKey("OutboundTripId")]
        [InverseProperty("OutboundDocuments")]
        public OutboundTripMaster OutboundTrip { get; set; }
        [ForeignKey("ProjectId")]
        [InverseProperty("OutboundDocuments")]
        public Project Project { get; set; }
        [InverseProperty("Document")]
        public ICollection<OutboundItem> OutboundItems { get; set; }
    }
}
