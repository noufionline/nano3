﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Nano3.Core.Tracking
{
  /// <summary>
    /// Change-tracking state of an entity.
    /// </summary>
    public enum TrackingState
    {
        /// <summary>Existing entity that has not been modified.</summary>
        Unchanged,
        /// <summary>Newly created entity.</summary>
        Added,
        /// <summary>Existing entity that has been modified.</summary>
        Modified,
        /// <summary>Existing entity that has been marked as deleted.</summary>
        Deleted
    }
}
