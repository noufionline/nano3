using Jasmine.Core.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jasmine.Core.Tracking
{
     public class ChangeTracker<T> where T : class, IEntity, ITrackable, INotifyPropertyChanged,IDirty,IIdentifiable
    {
        private ChangeTrackingCollection<T> _changeTracker;

        public event EventHandler EntityChanged;

        protected void OnEntityChanged()
        {
            EntityChanged?.Invoke(this,EventArgs.Empty);
        }
       

        public void TryAddExcludedProperty(string property)
        {
            if (!_changeTracker.ExcludedProperties.Contains(property))
            {
                _changeTracker.ExcludedProperties.Add(property);
            }
        }


        
        public void StartTracking(T entity,params string[] excludedProperties)
        {
            _changeTracker = new ChangeTrackingCollection<T>(entity);

            if (excludedProperties != null)
            {
                foreach (string property in excludedProperties)
                {
                    if (!_changeTracker.ExcludedProperties.Contains(property))
                    {
                        _changeTracker.ExcludedProperties.Add(property);
                    }
                }
            }

            _changeTracker.EntityChanged += (s, e) =>
            {
                if (e.PropertyName != "IsDirty")
                {
                    entity.MakeDirty(true, e.PropertyName);
                    OnEntityChanged();
                }

            };

            _changeTracker.Tracking = true;

        }


        public T GetModifiedEntity()
        {
             return _changeTracker.GetChanges().SingleOrDefault();
        }

        public void MergeChanges(T entity)
        {
            _changeTracker.MergeChanges(entity);
        }
    }
}
