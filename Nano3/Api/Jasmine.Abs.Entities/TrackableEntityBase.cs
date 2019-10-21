using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TrackableEntities.Common.Core;

namespace Jasmine.Abs.Entities
{
    public abstract class TrackableEntityBase:ITrackable,IMergeable
    {
        [NotMapped]
        public TrackingState TrackingState { get; set; }

        [NotMapped] 
        public ICollection<string> ModifiedProperties { get; set; } = new List<string>();
        [NotMapped]
        public Guid EntityIdentifier { get; set; }
    }
}