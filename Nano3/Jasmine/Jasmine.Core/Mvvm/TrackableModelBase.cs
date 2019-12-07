using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Jasmine.Core.Audit;
using Jasmine.Core.Contracts;
using Jasmine.Core.Tracking;


namespace Jasmine.Core.Mvvm
{
    public abstract class TrackableModelBase:IEntity,ITrackable,IMergeable
    {
        [NotMapped]
        [IgnoreTracking]
        public TrackingState TrackingState { get; set; }

        [NotMapped]
        [IgnoreTracking]
        public ICollection<string> ModifiedProperties { get; set; }

        [NotMapped]
        [IgnoreTracking]
        public Guid EntityIdentifier { get; set; }

        public int Id { get; set; }
    }
}