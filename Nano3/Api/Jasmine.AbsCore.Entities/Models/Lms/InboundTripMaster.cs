using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Lms
{
[Table("InboundTripMaster")]
    public partial class InboundTripMaster:TrackableEntityBase, IEntity
    {
        public InboundTripMaster()
        {
            InboundDocuments = new HashSet<InboundDocument>();
            InboundTripImages = new HashSet<InboundTripImage>();
        }

        [Key]
        [Column("InboundTripId")]
        public int Id { get; set; }
        public int SecurityGateId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime TripDate { get; set; }
        public int VehicleId { get; set; }
        public int DriverId { get; set; }
        public bool WithoutMaterials { get; set; }
        public int? PurposeId { get; set; }
        [StringLength(500)]
        public string Remarks { get; set; }
        public int? UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastModified { get; set; }
        [StringLength(250)]
        public string CameraRemarks { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal TripCharge { get; set; }
        public int? TripPayerId { get; set; }
        public int? ChargedDivisionId { get; set; }
        public int? SourceId { get; set; }
        public bool Matched { get; set; }
        [StringLength(250)]
        public string TripRemarks { get; set; }

        [ForeignKey("ChargedDivisionId")]
        [InverseProperty("InboundTripMasters")]
        public Division ChargedDivision { get; set; }
        [ForeignKey("DriverId")]
        [InverseProperty("InboundTripMasters")]
        public Driver Driver { get; set; }
        [ForeignKey("PurposeId")]
        [InverseProperty("InboundTripMasters")]
        public TripPurpose Purpose { get; set; }
        [ForeignKey("SecurityGateId")]
        [InverseProperty("InboundTripMasters")]
        public SecurityGate SecurityGate { get; set; }
        [ForeignKey("SourceId")]
        [InverseProperty("InboundTripMasters")]
        public Location Source { get; set; }
        [ForeignKey("TripPayerId")]
        [InverseProperty("InboundTripMasters")]
        public TripPayer TripPayer { get; set; }
        [ForeignKey("VehicleId")]
        [InverseProperty("InboundTripMasters")]
        public VehicleMaster Vehicle { get; set; }
        [InverseProperty("InboundTrip")]
        public InboundTripCharge InboundTripCharge { get; set; }
        [InverseProperty("InboundTrip")]
        public ICollection<InboundDocument> InboundDocuments { get; set; }
        [InverseProperty("InboundTrip")]
        public ICollection<InboundTripImage> InboundTripImages { get; set; }
    }
}
