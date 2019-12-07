using System;
using Jasmine.Core.Tracking;

namespace Jasmine.Core.Contracts
{
    public interface IDirty
    {
        bool IsDirty { get; }

        void MakeDirty(bool status = true, string propertyName=null);

        event EventHandler<DirtyChangeEventArgs> DirtyChanged ;

        void StartDirtyTracking(bool status=true);
    }



    
   
   
}
