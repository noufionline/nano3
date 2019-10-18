using Nano3.Core.Tracking;
using System.Collections.Generic;

namespace Nano3.Core.Contracts
{
    public interface IEntity
    {
        int Id { get; set; }
    }

     /// <summary>
    /// Interface implemented by entities that are change-tracked.
    /// </summary>
    public interface ITrackable
    {
        /// <summary>
        /// Change-tracking state of an entity.
        /// </summary>
        TrackingState TrackingState { get; set; }

        /// <summary>
        /// Properties on an entity that have been modified.
        /// </summary>
        ICollection<string> ModifiedProperties { get; set; }
    }


}