using System.Windows;
using System.Windows.Forms;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Printing;
using DevExpress.XtraReports;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
using IDialogService = Prism.Services.Dialogs.IDialogService;
using DialogService = Prism.Services.Dialogs.DialogService;

namespace Jasmine.Core.Dialogs
{
    //public class JasmineDialogService : DialogService, IDialogService
    //{
    //    public JasmineDialogService(IContainerExtension containerExtension)
    //       : base(containerExtension)
    //    {
    //    }
    //    public bool? ShowDialog(object uri, DialogParameters parameters = null)
    //    {
    //        ButtonResult result =ButtonResult.None;


    //        ShowDialog(uri.ToString(), parameters, dr =>
    //         {
    //             result = dr.Result;
    //         });

    //        if(result==ButtonResult.None)
    //            return null;

    //        return result==ButtonResult.OK ;
    //    }

    //    public MessageBoxResult ShowMessageBox(string messageBoxText, string caption, MessageBoxButton button,
    //        MessageBoxImage icon,
    //        MessageBoxResult defaultResult)
    //    {
    //        return ThemedMessageBox.Show( caption,messageBoxText, button, icon);
    //    }

    //    public void ShowReport(IReport report)
    //    {
    //        DocumentPreviewWindow window = new DocumentPreviewWindow();
    //      //  report.CreateDocument();
    //        window.PreviewControl.DocumentSource = report;
    //        window.Show();
    //    }
    //}
}