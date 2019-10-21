using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Lms
{
    public partial class OutboundTripCharge:TrackableEntityBase
    {
        [Key]
        public int OutboundTripId { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal TripCharge { get; set; }
        public int? TripPayerId { get; set; }
        [StringLength(250)]
        public string TripRemarks { get; set; }
        public int UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime LastModified { get; set; }

        [ForeignKey("OutboundTripId")]
        [InverseProperty("OutboundTripCharge")]
        public OutboundTripMaster OutboundTrip { get; set; }
    }
}
