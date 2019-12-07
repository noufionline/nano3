using Jasmine.Core.Dialogs;
using System;

namespace Jasmine.Core.Contracts
{
    public interface IAbsDialogAware
    {
        bool CanCloseDialog();

        event Action RequestWindowClose;

        DialogOptions GetDialogOptions();

        bool? DialogResult { get; }

        bool ClosingFromViewModel { get; }

        void OnClosed();

    }


    //public interface IDialogAwareAsync:IDialogAware
    //{

    //    Task<bool> SaveBeforeClosingAsync();

    //    bool SaveBeforeClosingRequested { get; set; }
    //}
}
