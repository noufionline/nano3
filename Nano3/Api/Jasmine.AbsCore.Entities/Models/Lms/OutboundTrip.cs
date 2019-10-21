using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Lms
{
    public partial class OutboundTrip:TrackableEntityBase
    {
        [Key]
        public int TripId { get; set; }
        public int OutboundTripId { get; set; }
        public int ChargedDivisionId { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal TripCharges { get; set; }

        [ForeignKey("OutboundTripId")]
        [InverseProperty("OutboundTrips")]
        public OutboundTripMaster OutboundTripNavigation { get; set; }
    }
}
