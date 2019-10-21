using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Lms
{
    public partial class User:TrackableEntityBase
    {
        public User()
        {
            OutboundTripMasters = new HashSet<OutboundTripMaster>();
        }

        public int UserId { get; set; }
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }
        [Required]
        [StringLength(10)]
        public string Password { get; set; }
        public int? DivisionId { get; set; }

        [InverseProperty("User")]
        public ICollection<OutboundTripMaster> OutboundTripMasters { get; set; }
    }
}
