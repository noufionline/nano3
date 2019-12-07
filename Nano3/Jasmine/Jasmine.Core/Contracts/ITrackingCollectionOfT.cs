using System.Collections.Generic;
using System.ComponentModel;
using Jasmine.Core.Tracking;

namespace Jasmine.Core.Contracts
{
    /// <summary>
    /// Interface implemented by trackable collections.
    /// </summary>
    public interface ITrackingCollection<TEntity> : ICollection<TEntity>
        where TEntity : class,IEntity,ITrackable, INotifyPropertyChanged
    {
        /// <summary>
        /// Get entities that have been marked as Added, Modified or Deleted.
        /// </summary>
        /// <returns>Collection containing only changed entities</returns>
        ChangeTrackingCollection<TEntity> GetChanges();
    }
}
