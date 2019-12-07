using Prism.Services.Dialogs;
using System;

namespace Jasmine.Core.Dialogs
{
    public interface IRegionDialogService
    {
        void Show(string name, IDialogParameters dialogParameters, Action<IDialogResult> callback);
        void ShowDialog(string name, IDialogParameters parameters, Action<IDialogResult> callback);
    }

  
}