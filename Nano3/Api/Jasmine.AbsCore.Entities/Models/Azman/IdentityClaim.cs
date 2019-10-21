using System;
using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Azman
{
    public partial class IdentityClaim:TrackableEntityBase
    {
        public int Id { get; set; }
        public int IdentityResourceId { get; set; }
        public string Type { get; set; }

        public IdentityResource IdentityResource { get; set; }
    }
}
