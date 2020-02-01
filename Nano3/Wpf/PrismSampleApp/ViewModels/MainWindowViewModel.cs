using DevExpress.Mvvm;
using GrpcService;
using PostSharp.Patterns.Model;
using Prism.Commands;
using Prism.Mvvm;
using PrismSampleApp.Mapper;
using PrismSampleApp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Windows;
using DelegateCommand = Prism.Commands.DelegateCommand;

namespace PrismSampleApp.ViewModels
{
    [NotifyPropertyChanged]
    public class MainWindowViewModel
    {
        private readonly IAlfrescoClient _alfrescoClient;
        private readonly ICustomerService _service;
        private readonly ISignInManager _signInManager;
        private readonly IApiTokenProvider _tokenProvider;

        public MainWindowViewModel(IAlfrescoClient alfrescoClient, 
            ICustomerService service, ISignInManager signInManager,IApiTokenProvider tokenProvider)
        {
            OpenFileCommand = new AsyncCommand<string>(ExecuteOpenFile, CanExecuteOpenFile);
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
            var principal = await _signInManager.SignInAsync("noufal@cicon.net", "MtpsF42@", 1);
            AppDomain.CurrentDomain.SetThreadPrincipal(principal);
            MessageBox.Show("Login Success");
        }

        #endregion

        #region FetchDataCommand

        public AsyncCommand<string> FetchDataCommand { get; set; }

        protected bool CanExecuteFetchData(string dbName) => dbName!=null;

        private async Task ExecuteFetchDataAsync(string dbName)
        {  
            var pricipal = ClaimsPrincipal.Current;
            Customers = await _service.GetAllAsync(dbName);
        }

        public List<CustomerList> Customers { get; set; }

        #endregion

        #region OpenFileCommand

        public AsyncCommand<string> OpenFileCommand { get; set; }

        protected bool CanExecuteOpenFile(string db) => !string.IsNullOrWhiteSpace(db);

        private async Task ExecuteOpenFile(string db)
        {
            //var path=@"C:\Users\Noufal\Downloads\41092_0.pdf";
            //(Guid id, string version) result = await _alfrescoClient.AttachFileAsync(Guid.Parse("75faed7f-1bba-4877-aff5-a2fd37efd9bd"), "ACC-1234567.pdf", path,
            //    new FileOptions
            //    {
            //        //Title = "Title",
            //        //Comment = "Comment",
            //        RelativePath = $"ACC/SHAMS",
            //        //Description = "Description",
            //        //MimeType = "application/pdf",
            //        Overwrite = true
            //    });
            //// await _alfrescoClient.OpenFileAsync("8a4c343e-af89-4c84-94d8-cc431426be7a");
            // await _alfrescoClient.OpenFileAsync(result.id);

           // var items= await _service.GetDeliveryDetailsReportDataAsync(new SteelDeliveryNoteDetailReportCriteriaRequest{ DbName=db });

            var path=@"C:\Users\Noufal\Downloads\test.pdf";
            var sm=await _service.GetFileAsync();
           
            await File.WriteAllBytesAsync(path,sm);
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
