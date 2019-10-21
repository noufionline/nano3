using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Lms
{
    public partial class Driver:TrackableEntityBase
    {
        public Driver()
        {
            InboundTripMasters = new HashSet<InboundTripMaster>();
            OutboundTripMasters = new HashSet<OutboundTripMaster>();
        }

        public int DriverId { get; set; }
        [Required]
        [StringLength(50)]
        public string DriverName { get; set; }
        public int? EmployeeId { get; set; }
        public bool InHouse { get; set; }
        public bool Exclude { get; set; }

        [InverseProperty("Driver")]
        public ICollection<InboundTripMaster> InboundTripMasters { get; set; }
        [InverseProperty("Driver")]
        public ICollection<OutboundTripMaster> OutboundTripMasters { get; set; }
    }
}
