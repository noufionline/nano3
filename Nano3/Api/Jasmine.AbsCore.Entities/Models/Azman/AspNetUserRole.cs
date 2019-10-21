using System;
using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Azman
{
    public partial class AspNetUserRole:TrackableEntityBase
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }

        public AspNetRole Role { get; set; }
        public AspNetUser User { get; set; }
    }
}
