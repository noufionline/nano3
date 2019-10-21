using System;
using System.Collections.Generic;

namespace Jasmine.AbsCore.Entities.Models.Azman
{
    public partial class ApiClaim:TrackableEntityBase
    {
        public int Id { get; set; }
        public int ApiResourceId { get; set; }
        public string Type { get; set; }

        public ApiResource ApiResource { get; set; }
    }
}
