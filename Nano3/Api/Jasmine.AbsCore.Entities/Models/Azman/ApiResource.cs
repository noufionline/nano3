using System;
using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Azman
{
    public partial class ApiResource:TrackableEntityBase
    {
        public ApiResource()
        {
            ApiClaims = new HashSet<ApiClaim>();
            ApiScopes = new HashSet<ApiScope>();
            ApiSecrets = new HashSet<ApiSecret>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }

        public ICollection<ApiClaim> ApiClaims { get; set; }
        public ICollection<ApiScope> ApiScopes { get; set; }
        public ICollection<ApiSecret> ApiSecrets { get; set; }
    }
}
