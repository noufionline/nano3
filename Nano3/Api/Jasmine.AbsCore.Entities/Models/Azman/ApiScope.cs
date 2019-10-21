using System;
using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Azman
{
    public partial class ApiScope:TrackableEntityBase
    {
        public ApiScope()
        {
            ApiScopeClaims = new HashSet<ApiScopeClaim>();
        }

        public int Id { get; set; }
        public int ApiResourceId { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public bool Emphasize { get; set; }
        public string Name { get; set; }
        public bool Required { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }

        public ApiResource ApiResource { get; set; }
        public ICollection<ApiScopeClaim> ApiScopeClaims { get; set; }
    }
}
