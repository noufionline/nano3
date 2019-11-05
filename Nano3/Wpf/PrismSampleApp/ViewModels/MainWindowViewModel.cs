using DevExpress.Mvvm;
using Prism.Commands;
using Prism.Mvvm;
using PrismSampleApp.Services;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PrismSampleApp.ViewModels
{
   // [NotifyPropertyChanged]
    public class MainWindowViewModel
    {
        private readonly IAlfrescoClient _alfrescoClient;

        public MainWindowViewModel(IAlfrescoClient alfrescoClient)
        {
            OpenFileCommand = new AsyncCommand(ExecuteOpenFile, CanExecuteOpenFile);
            _alfrescoClient = alfrescoClient;
        }


        #region OpenFileCommand

        public AsyncCommand OpenFileCommand { get; set; }

        protected bool CanExecuteOpenFile() => true;

        private async Task ExecuteOpenFile()
        {
            var userName="admin";
            var password ="MtpsF42@Alfresco";
            await _alfrescoClient.OpenFileAsync("8a4c343e-af89-4c84-94d8-cc431426be7a",userName,password);
        }

        #endregion
    }
}
