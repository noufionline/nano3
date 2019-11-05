using System;
using System.Collections.Generic;

namespace Jasmine.Abs.Entities.Models.Azman
{
    public partial class IdentityResource : TrackableEntityBase
    {
        public IdentityResource()
        {
            IdentityClaims = new HashSet<IdentityClaim>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public bool Emphasize { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public bool Required { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }

        public virtual ICollection<IdentityClaim> IdentityClaims { get; set; }
    }
}
