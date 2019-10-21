using System;
using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Azman
{
    public partial class AspNetUserClaim:TrackableEntityBase
    {
        public int Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public string UserId { get; set; }

        public AspNetUser User { get; set; }
    }
}
