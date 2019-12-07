using DevExpress.Xpf.Printing;
using DevExpress.XtraReports;
using Prism.Services.Dialogs;
using System;
using IDialogService=Prism.Services.Dialogs.IDialogService;
namespace Jasmine.Core.Helpers
{
    public static class PrismDialogServiceExtensions
    {
        public static void ShowDialog(this IDialogService dialogService,string name,IDialogParameters parameters=null,Action<IDialogResult> callback=null)
        {
            dialogService.ShowDialog(name,parameters,callback);
        }


         public static void ShowReport(this IDialogService dialogService,IReport report)
         {
            DocumentPreviewWindow window = new DocumentPreviewWindow();
            //  report.CreateDocument();
            window.PreviewControl.DocumentSource = report;
            window.Show();
         }
    }
}
