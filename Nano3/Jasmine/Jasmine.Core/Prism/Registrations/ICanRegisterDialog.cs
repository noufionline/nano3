using Prism.Services.Dialogs;

namespace Jasmine.Core.Prism.Registrations
{
    public interface ICanRegisterDialog
    {
        ICanRegisterDialogAndService WithDialog<T>() where T : class;
        ICanRegisterDialogAndService WithDialog<TView,TViewModel>() where TView : class where TViewModel:class, IDialogAware;
    }
}