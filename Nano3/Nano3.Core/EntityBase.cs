using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.JsonPatch;
using Nano3.Core.Contracts;
using Nano3.Core.Events;
using Nano3.Core.Tracking;
using PostSharp.Patterns.Model;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Nano3.Core
{
    [NotifyPropertyChanged]
    public abstract class EntityBase<T> : EntityBase,INotifyDataErrorInfo, ISupportPatchUpdate,ISupportFluentValidator<T>
        where T : IEntity, IDirty
    {

         /// <summary>
        /// Gets the errors container.
        /// </summary>
        /// <value>The errors container.</value>
        public ErrorsContainer<ValidationFailure> ErrorsContainer { get; }
        public void SetValidators(IValidator<T> validator)
        {
            throw new NotImplementedException();
        }

        public void ValidateSelf(string propertyName = null)
        {
            throw new NotImplementedException();
        }

        public void SetError(string propertyName, string[] errors)
        {
            throw new NotImplementedException();
        }

        public void SetErrors(IList<ValidationFailure> errors)
        {
            throw new NotImplementedException();
        }

        public List<ValidationFailure> ValidationSummary { get; }

        public JsonPatchDocument CreatePatchDocument()
        {
            throw new NotImplementedException();
        }

        public IEnumerable GetErrors(string propertyName)
        {
            throw new NotImplementedException();
        }

        public bool HasErrors { get; }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    }

    [NotifyPropertyChanged]
    public abstract class EntityBase : INotifyPropertyChanged, IEntity, IDirty, ITrackable, IIdentifiable, IMergeable
    {


        public EntityBase()
        {
            ModifiedProperties = new HashSet<string>();

            _excludedPropertiesFromDirtyTracking = new HashSet<string>();
            _excludedPropertiesFromDirtyTracking.Add(nameof(EntityIdentifier));
            _excludedPropertiesFromDirtyTracking.Add(nameof(TrackingState));
            _excludedPropertiesFromDirtyTracking.Add(nameof(ModifiedProperties));
            _excludedPropertiesFromDirtyTracking.Add(nameof(IsDirty));
            _excludedPropertiesFromDirtyTracking.Add("HasErrors");
        }


        private HashSet<string> _excludedPropertiesFromDirtyTracking;

        protected HashSet<string> ExcludedPropertiesFromDirtyTracking
        {
            get
            {
                return _excludedPropertiesFromDirtyTracking;
            }
        }

        private bool _dirtyTracking;
        private bool _isDirty;

        public event PropertyChangedEventHandler PropertyChanged;


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
            IsDirty = true;
        }

        public event EventHandler<DirtyChangeEventArgs> DirtyChanged;

        public void StartDirtyTracking(bool status = true)
        {
            _dirtyTracking = status;
        }


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

        protected virtual void OnDirtyChanged(DirtyChangeEventArgs e)
        {
            DirtyChanged?.Invoke(this, e);
        }

        public TrackingState TrackingState { get; set; }
        public ICollection<string> ModifiedProperties { get; set; }

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

        public Guid EntityIdentifier { get; set; }
    }

}
