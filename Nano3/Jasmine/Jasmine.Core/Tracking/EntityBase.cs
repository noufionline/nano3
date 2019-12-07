using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using DevExpress.PivotGrid.DataCalculation;
using Jasmine.Core.Audit;
using Jasmine.Core.Contracts;
using PostSharp.Patterns.Model;

namespace Jasmine.Core.Tracking
{
    /// <summary>
    /// Base class for model entities
    /// </summary>
    [NotifyPropertyChanged]
    public abstract partial class EntityBase : INotifyPropertyChanged, IEntity, IDirty, ITrackable, IIdentifiable, IMergeable
    {
        private bool _isDirty;
        private bool _dirtyTracking;

        protected EntityBase()
        {
            ModifiedProperties = new HashSet<string>();
            _excludedPropertiesFromDirtyTracking =new HashSet<string>();
            _excludedPropertiesFromDirtyTracking.Add(nameof(EntityIdentifier));
            _excludedPropertiesFromDirtyTracking.Add(nameof(TrackingState));
            _excludedPropertiesFromDirtyTracking.Add(nameof(ModifiedProperties));
            _excludedPropertiesFromDirtyTracking.Add(nameof(IsDirty));
            _excludedPropertiesFromDirtyTracking.Add("HasErrors");

        }
        /// <summary>
        /// Event for notification of property changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Fire PropertyChanged event.
        /// </summary>
        /// <typeparam name="TResult">Property return type</typeparam>
        /// <param name="property">Lambda expression for property</param>
        protected void OnPropertyChanged<TResult>(Expression<Func<TResult>> property)
        {
            string propertyName = ((MemberExpression)property.Body).Member.Name;
            // ReSharper disable once ExplicitCallerInfoArgument
            OnPropertyChanged(propertyName);
        }


        private HashSet<string> _excludedPropertiesFromDirtyTracking;

        protected HashSet<string> ExcludedPropertiesFromDirtyTracking
        {
            get
            {
                return _excludedPropertiesFromDirtyTracking;
            }
        }

        /// <summary>
        /// Fire PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (!ExcludedPropertiesFromDirtyTracking.Contains(propertyName))
            {
                if (!IsDirty && _dirtyTracking)
                {
                    MakeDirty(true, propertyName);
                }
            }
        }



        /// <summary>
        /// Generate entity identifier used for correlation with MergeChanges (if not yet done)
        /// </summary>
        public void SetEntityIdentifier()
        {
            if (EntityIdentifier == Guid.Empty)
                EntityIdentifier = Guid.NewGuid();
        }

        /// <summary>
        /// Copy entity identifier used for correlation with MergeChanges from another entity
        /// </summary>
        /// <param name="other">Other trackable object</param>
        public void SetEntityIdentifier(IIdentifiable other)
        {
            if (other is EntityBase otherEntity)
                EntityIdentifier = otherEntity.EntityIdentifier;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same
        /// type. The comparison is based on EntityIdentifier.
        /// 
        /// If the local EntityIdentifier is empty, then return false.
        /// </summary>
        /// <param name="other">An object to compare with this object</param>
        /// <returns></returns>
        public bool IsEquatable(IIdentifiable other)
        {
            if (EntityIdentifier == default(Guid))
                return false;

            if (!(other is EntityBase otherEntity))
                return false;

            return EntityIdentifier.Equals(otherEntity.EntityIdentifier);
        }

        bool IEquatable<IIdentifiable>.Equals(IIdentifiable other)
        {
            return IsEquatable(other);
        }


        public int Id { get; set; }

        public bool IsDirty
        {
            get => _isDirty;
            private set
            {
                if (_dirtyTracking)
                {
                    if (_isDirty == value) return;
                    bool oldValue = _isDirty;
                    _isDirty = value;
                    OnPropertyChanged();
                    OnDirtyChanged(new DirtyChangeEventArgs(oldValue, value));
                }
            }
        }

        public void MakeDirty(bool status = true, string propertyName = null)
        {
            if (propertyName != null)
            {
                Debug.WriteLine($"Changed Property : {propertyName}");
            }

            IsDirty = status;
        }

        public event EventHandler<DirtyChangeEventArgs> DirtyChanged;
        public void StartDirtyTracking(bool status = true)
        {
            _dirtyTracking = status;
        }


        protected virtual void OnDirtyChanged(DirtyChangeEventArgs e)
        {
            DirtyChanged?.Invoke(this, e);
        }


    }

    public class DirtyChangeEventArgs : EventArgs
    {
        public bool OldValue { get; }
        public bool NewValue { get; }

        public DirtyChangeEventArgs(bool oldValue, bool newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}