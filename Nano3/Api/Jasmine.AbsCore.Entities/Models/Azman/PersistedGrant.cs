using System;
using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Azman
{
    public partial class PersistedGrant:TrackableEntityBase
    {
        public string Key { get; set; }
        public string Type { get; set; }
        public string ClientId { get; set; }
        public DateTime CreationTime { get; set; }
        public string Data { get; set; }
        public DateTime? Expiration { get; set; }
        public string SubjectId { get; set; }
    }
}
