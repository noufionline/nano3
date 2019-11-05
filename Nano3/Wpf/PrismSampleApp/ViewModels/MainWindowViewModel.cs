using DevExpress.Mvvm;
using GrpcService;
using PostSharp.Patterns.Model;
using Prism.Commands;
using Prism.Mvvm;
using PrismSampleApp.Mapper;
using PrismSampleApp.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PrismSampleApp.ViewModels
{
   [NotifyPropertyChanged]
    public class MainWindowViewModel
    {
        private readonly IAlfrescoClient _alfrescoClient;
        private readonly ICustomerService _service;
        private readonly ISignInManager _signInManager;

        public MainWindowViewModel(IAlfrescoClient alfrescoClient,ICustomerService service,ISignInManager signInManager)
        {
            OpenFileCommand = new AsyncCommand(ExecuteOpenFile, CanExecuteOpenFile);
            _alfrescoClient = alfrescoClient;
            _service = service;
            _signInManager = signInManager;
            FetchDataCommand = new AsyncCommand(ExecuteFetchDataAsync, CanExecuteFetchData);
        }



        #region FetchDataCommand

        public AsyncCommand FetchDataCommand { get; set; }

        protected bool CanExecuteFetchData() => true;

        private async Task ExecuteFetchDataAsync()
        {
             var result = await _signInManager.SignInAsync("noufal@cicon.net", "MtpsF42@", 1);

            Customers = await _service.GetAllAsync();
        }

        public List<CustomerList> Customers{get;set;}

        #endregion

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
