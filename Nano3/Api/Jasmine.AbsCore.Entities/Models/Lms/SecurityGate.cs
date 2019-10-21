using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Lms
{
    public partial class SecurityGate:TrackableEntityBase
    {
        public SecurityGate()
        {
            InboundTripMasters = new HashSet<InboundTripMaster>();
            OutboundTripMasters = new HashSet<OutboundTripMaster>();
        }

        public int SecurityGateId { get; set; }
        [Required]
        [StringLength(150)]
        public string SecurityGateName { get; set; }

        [InverseProperty("SecurityGate")]
        public ICollection<InboundTripMaster> InboundTripMasters { get; set; }
        [InverseProperty("SecurityGate")]
        public ICollection<OutboundTripMaster> OutboundTripMasters { get; set; }
    }
}
