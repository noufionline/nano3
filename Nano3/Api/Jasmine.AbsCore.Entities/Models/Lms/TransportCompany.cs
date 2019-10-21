using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Lms
{
    public partial class TransportCompany:TrackableEntityBase
    {
        public TransportCompany()
        {
            VehicleMasters = new HashSet<VehicleMaster>();
        }

        public int TransportCompanyId { get; set; }
        [Required]
        [StringLength(250)]
        public string TransportCompanyName { get; set; }
        public bool InHouse { get; set; }

        [InverseProperty("TransportCompany")]
        public ICollection<VehicleMaster> VehicleMasters { get; set; }
    }
}
