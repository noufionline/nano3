using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Lms
{
[Table("OutboundTripMaster")]
    public partial class OutboundTripMaster:TrackableEntityBase
    {
        public OutboundTripMaster()
        {
            OutboundDocuments = new HashSet<OutboundDocument>();
            OutboundTripImages = new HashSet<OutboundTripImage>();
            OutboundTrips = new HashSet<OutboundTrip>();
        }

        [Key]
        [Column("OutboundTripId")]
        public int Id { get; set; }
        public int SecurityGateId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime TripDate { get; set; }
        public int VehicleId { get; set; }
        public int DriverId { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal WeighBridgeWeight { get; set; }
        public bool WithoutMaterials { get; set; }
        public int? PurposeId { get; set; }
        [Required]
        [StringLength(500)]
        public string Remarks { get; set; }
        public bool HasDiscrepancy { get; set; }
        public int? UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime LastModified { get; set; }
        [StringLength(250)]
        public string CameraRemarks { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal TripCharge { get; set; }
        public int? TripPayerId { get; set; }
        public int? ChargedDivisionId { get; set; }
        public int? DestinationId { get; set; }
        public bool Matched { get; set; }
        [StringLength(250)]
        public string TripRemarks { get; set; }

        [ForeignKey("ChargedDivisionId")]
        [InverseProperty("OutboundTripMasters")]
        public Division ChargedDivision { get; set; }
        [ForeignKey("DestinationId")]
        [InverseProperty("OutboundTripMasters")]
        public Location Destination { get; set; }
        [ForeignKey("DriverId")]
        [InverseProperty("OutboundTripMasters")]
        public Driver Driver { get; set; }
        [ForeignKey("PurposeId")]
        [InverseProperty("OutboundTripMasters")]
        public TripPurpose Purpose { get; set; }
        [ForeignKey("SecurityGateId")]
        [InverseProperty("OutboundTripMasters")]
        public SecurityGate SecurityGate { get; set; }
        [ForeignKey("TripPayerId")]
        [InverseProperty("OutboundTripMasters")]
        public TripPayer TripPayer { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("OutboundTripMasters")]
        public User User { get; set; }
        [ForeignKey("VehicleId")]
        [InverseProperty("OutboundTripMasters")]
        public VehicleMaster Vehicle { get; set; }
        [InverseProperty("OutboundTrip")]
        public OutboundTripCharge OutboundTripCharge { get; set; }
        [InverseProperty("OutboundTrip")]
        public ICollection<OutboundDocument> OutboundDocuments { get; set; }
        [InverseProperty("OutboundTrip")]
        public ICollection<OutboundTripImage> OutboundTripImages { get; set; }
        [InverseProperty("OutboundTripNavigation")]
        public ICollection<OutboundTrip> OutboundTrips { get; set; }
    }
}
