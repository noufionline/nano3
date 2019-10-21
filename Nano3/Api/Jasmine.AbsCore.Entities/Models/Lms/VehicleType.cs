using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Lms
{
    public partial class VehicleType:TrackableEntityBase
    {
        public VehicleType()
        {
            VehicleMasters = new HashSet<VehicleMaster>();
        }

        public int VehicleTypeId { get; set; }
        [Required]
        [Column("VehicleType")]
        [StringLength(50)]
        public string VehicleType1 { get; set; }
        public bool IsPayable { get; set; }

        [InverseProperty("VehicleType")]
        public ICollection<VehicleMaster> VehicleMasters { get; set; }
    }
}
