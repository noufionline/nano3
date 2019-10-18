using System;

namespace Nano3.Core.Events
{
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