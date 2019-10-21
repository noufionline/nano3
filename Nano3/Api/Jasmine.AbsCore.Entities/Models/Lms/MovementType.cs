using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Lms
{
    public partial class MovementType:TrackableEntityBase
    {
        public MovementType()
        {
            InboundDocuments = new HashSet<InboundDocument>();
            OutboundDocuments = new HashSet<OutboundDocument>();
        }

        public int MovementTypeId { get; set; }
        [Required]
        [Column("MovementType")]
        [StringLength(50)]
        public string MovementType1 { get; set; }
        [StringLength(3)]
        public string Direction { get; set; }
        public int? OutboundMovementTypeId { get; set; }
        public int? OutboundDeliveryTypeId { get; set; }

        [InverseProperty("MovementType")]
        public ICollection<InboundDocument> InboundDocuments { get; set; }
        [InverseProperty("MovementType")]
        public ICollection<OutboundDocument> OutboundDocuments { get; set; }
    }
}
