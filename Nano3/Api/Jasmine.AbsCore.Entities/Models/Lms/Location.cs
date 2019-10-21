using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Lms
{
    public partial class Location:TrackableEntityBase
    {
        public Location()
        {
            InboundDocuments = new HashSet<InboundDocument>();
            InboundTripMasters = new HashSet<InboundTripMaster>();
            OutboundDocuments = new HashSet<OutboundDocument>();
            OutboundTripMasters = new HashSet<OutboundTripMaster>();
        }

        public int LocationId { get; set; }
        [Required]
        [StringLength(50)]
        public string LocationName { get; set; }
        public int? TripLocationId { get; set; }

        [InverseProperty("Location")]
        public ICollection<InboundDocument> InboundDocuments { get; set; }
        [InverseProperty("Source")]
        public ICollection<InboundTripMaster> InboundTripMasters { get; set; }
        [InverseProperty("Location")]
        public ICollection<OutboundDocument> OutboundDocuments { get; set; }
        [InverseProperty("Destination")]
        public ICollection<OutboundTripMaster> OutboundTripMasters { get; set; }
    }
}
