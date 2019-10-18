using Nano3.Core.Contracts;
using Nano3.Core.Events;
using Nano3.Core.Tracking;
using PostSharp.Patterns.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Nano3.Core
{
    [NotifyPropertyChanged]
    public abstract class EntityBase<T> : EntityBase where T : IEntity, IDirty
    {

    }


    public abstract class EntityBase : INotifyPropertyChanged, IEntity, IDirty,ITrackable,IIdentifiable,IMergeable
    {
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
            if (propertyName != nameof(IsDirty) && propertyName != "HasErrors"
                                                && propertyName != nameof(EntityIdentifier)
                                                && propertyName != nameof(TrackingState)
                                                && propertyName != nameof(ModifiedProperties))
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

        public void SetEntityIdentifier()
        {
            throw new NotImplementedException();
        }

        public void SetEntityIdentifier(IIdentifiable other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IIdentifiable other)
        {
            throw new NotImplementedException();
        }

        public Guid EntityIdentifier { get; set; }
    }

}
