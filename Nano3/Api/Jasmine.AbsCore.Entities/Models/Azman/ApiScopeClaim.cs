using System;
using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Azman
{
    public partial class ApiScopeClaim:TrackableEntityBase
    {
        public int Id { get; set; }
        public int ApiScopeId { get; set; }
        public string Type { get; set; }

        public ApiScope ApiScope { get; set; }
    }
}
