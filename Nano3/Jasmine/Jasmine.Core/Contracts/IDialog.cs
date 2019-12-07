using Prism.Services.Dialogs;

namespace Jasmine.Core.Contracts
{
    public interface IDialog
    {
        double Left { get; set; }
        double Top { get; set; }
        void Close();
        void Show();
    }

    public interface IRegionDialogWindow : IDialogWindow
    {

    }
}