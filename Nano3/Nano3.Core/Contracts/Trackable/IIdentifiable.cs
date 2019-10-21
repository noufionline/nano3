﻿using System;

namespace Nano3.Core.Contracts.Trackable
{
    /// <summary>
    /// Interface implemented by entities which perform MergeChanges
    /// </summary>
    public interface IIdentifiable : IEquatable<IIdentifiable>
    {
        /// <summary>
        /// Generate entity identifier used for correlation with MergeChanges (if not yet done)
        /// </summary>
        void SetEntityIdentifier();

        /// <summary>
        /// Copy entity identifier used for correlation with MergeChanges from another entity
        /// </summary>
        /// <param name="other">Other trackable object</param>
        void SetEntityIdentifier(IIdentifiable other);
    }


}