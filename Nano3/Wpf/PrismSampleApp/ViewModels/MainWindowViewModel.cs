using DevExpress.Mvvm;
using GrpcService;
using PostSharp.Patterns.Model;
using Prism.Commands;
using Prism.Mvvm;
using PrismSampleApp.Mapper;
using PrismSampleApp.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PrismSampleApp.ViewModels
{
    [NotifyPropertyChanged]
    public class MainWindowViewModel
    {
        private readonly IAlfrescoClient _alfrescoClient;
        private readonly ICustomerService _service;
        private readonly ISignInManager _signInManager;
        private readonly IApiTokenProvider _tokenProvider;

        public MainWindowViewModel(IAlfrescoClient alfrescoClient, ICustomerService service, ISignInManager signInManager,IApiTokenProvider tokenProvider)
        {
            OpenFileCommand = new AsyncCommand(ExecuteOpenFile, CanExecuteOpenFile);
            _alfrescoClient = alfrescoClient;
            _service = service;
            _signInManager = signInManager;
            _tokenProvider = tokenProvider;
            FetchDataCommand = new AsyncCommand<string>(ExecuteFetchDataAsync, CanExecuteFetchData);
            LoginCommand = new AsyncCommand(ExecuteLoginAsync, CanExecuteLogin);
        }


        public List<DivisionInfo> Divisions{get;set;} = DivisionInfo.GetAll();

        public string SelectedDb {get;set;}


        #region LoginCommand

        public AsyncCommand LoginCommand { get; set; }

        protected bool CanExecuteLogin()
        {
            return !_tokenProvider.HasToken;
        }

        private async Task ExecuteLoginAsync()
        {
            await _signInManager.SignInAsync("noufal@cicon.net", "MtpsF42@", 1);
        }

        #endregion

        #region FetchDataCommand

        public AsyncCommand<string> FetchDataCommand { get; set; }

        protected bool CanExecuteFetchData(string dbName) => dbName!=null;

        private async Task ExecuteFetchDataAsync(string dbName)
        {  
            Customers = await _service.GetAllAsync(dbName);
        }

        public List<CustomerList> Customers { get; set; }

        #endregion

        #region OpenFileCommand

        public AsyncCommand OpenFileCommand { get; set; }

        protected bool CanExecuteOpenFile() => true;

        private async Task ExecuteOpenFile()
        {
            var userName = "admin";
            var password = "MtpsF42@Alfresco";
            await _alfrescoClient.OpenFileAsync("8a4c343e-af89-4c84-94d8-cc431426be7a", userName, password);
        }

        #endregion
    }


    public class DivisionInfo
    {
        public string Name { get; set; }
        public string DbName { get; set; }

        public static List<DivisionInfo> GetAll()=> new List<DivisionInfo>
        {
            new DivisionInfo{Name ="Musaffah Store",DbName ="ABS_AUHStore"} ,   
            new DivisionInfo{Name ="Dubai Store",DbName ="ABS_DXBStore"} ,   
            new DivisionInfo{Name ="Factory 2",DbName ="ABS_CBF2"} ,   
            new DivisionInfo{Name ="Factory 4",DbName ="ABS_CBF4"} 
        };


    }
}
