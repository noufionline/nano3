using System;
using Nano3.Core.Events;
namespace Nano3.Core.Contracts
{
    public interface IDirty
    {
        bool IsDirty { get; }

        void MakeDirty(bool status = true, string propertyName = null);

        event EventHandler<DirtyChangeEventArgs> DirtyChanged;

        void StartDirtyTracking(bool status = true);
    }
}