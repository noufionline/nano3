using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Lms
{
    public partial class TripPayer:TrackableEntityBase
    {
        public TripPayer()
        {
            InboundTripMasters = new HashSet<InboundTripMaster>();
            OutboundTripMasters = new HashSet<OutboundTripMaster>();
        }

        public int TripPayerId { get; set; }
        [Required]
        [Column("TripPayer")]
        [StringLength(150)]
        public string TripPayer1 { get; set; }

        [InverseProperty("TripPayer")]
        public ICollection<InboundTripMaster> InboundTripMasters { get; set; }
        [InverseProperty("TripPayer")]
        public ICollection<OutboundTripMaster> OutboundTripMasters { get; set; }
    }
}
