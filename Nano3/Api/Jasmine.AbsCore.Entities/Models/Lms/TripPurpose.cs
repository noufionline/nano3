using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Lms
{
[Table("TripPurpose")]
    public partial class TripPurpose:TrackableEntityBase
    {
        public TripPurpose()
        {
            InboundTripMasters = new HashSet<InboundTripMaster>();
            OutboundTripMasters = new HashSet<OutboundTripMaster>();
        }

        [Key]
        public int PurposeId { get; set; }
        [Required]
        [StringLength(50)]
        public string Purpose { get; set; }

        [InverseProperty("Purpose")]
        public ICollection<InboundTripMaster> InboundTripMasters { get; set; }
        [InverseProperty("Purpose")]
        public ICollection<OutboundTripMaster> OutboundTripMasters { get; set; }
    }
}
