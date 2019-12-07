using System;

namespace Jasmine.Core.Contracts
{
    /// <summary>
    /// Provides an EntityIdentifier for correlation when merging changes.
    /// </summary>
    public interface IMergeable
    {
        /// <summary>
        /// Identifier used for correlation with MergeChanges.
        /// </summary>
        Guid EntityIdentifier { get; set; }
    }
}
