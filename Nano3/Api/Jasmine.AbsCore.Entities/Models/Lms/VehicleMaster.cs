using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Lms
{
[Table("VehicleMaster")]
    public partial class VehicleMaster:TrackableEntityBase
    {
        public VehicleMaster()
        {
            InboundTripMasters = new HashSet<InboundTripMaster>();
            OutboundTripMasters = new HashSet<OutboundTripMaster>();
        }

        [Key]
        public int VehicleId { get; set; }
        [Required]
        [StringLength(50)]
        public string TruckNo { get; set; }
        public int TransportCompanyId { get; set; }
        public int? PlateNo { get; set; }
        public int? RegistrationId { get; set; }
        public int VehicleTypeId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }

        [ForeignKey("RegistrationId")]
        [InverseProperty("VehicleMasters")]
        public VehicleRegistration Registration { get; set; }
        [ForeignKey("TransportCompanyId")]
        [InverseProperty("VehicleMasters")]
        public TransportCompany TransportCompany { get; set; }
        [ForeignKey("VehicleTypeId")]
        [InverseProperty("VehicleMasters")]
        public VehicleType VehicleType { get; set; }
        [InverseProperty("Vehicle")]
        public ICollection<InboundTripMaster> InboundTripMasters { get; set; }
        [InverseProperty("Vehicle")]
        public ICollection<OutboundTripMaster> OutboundTripMasters { get; set; }
    }
}
