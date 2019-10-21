using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Lms
{
    public partial class VehicleRegistration:TrackableEntityBase
    {
        public VehicleRegistration()
        {
            VehicleMasters = new HashSet<VehicleMaster>();
        }

        public int RegistrationId { get; set; }
        [Required]
        [StringLength(10)]
        public string Registration { get; set; }

        [InverseProperty("Registration")]
        public ICollection<VehicleMaster> VehicleMasters { get; set; }
    }
}
