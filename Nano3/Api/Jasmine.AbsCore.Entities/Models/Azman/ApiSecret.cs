using System;
using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Azman
{
    public partial class ApiSecret:TrackableEntityBase
    {
        public int Id { get; set; }
        public int ApiResourceId { get; set; }
        public string Description { get; set; }
        public DateTime? Expiration { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }

        public ApiResource ApiResource { get; set; }
    }
}
