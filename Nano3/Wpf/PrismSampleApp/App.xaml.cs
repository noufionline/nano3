using AutoMapper;
using DevExpress.Xpf.Core;
using Google.Protobuf.WellKnownTypes;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using Prism.Unity;
using PrismSampleApp.Services;
using PrismSampleApp.Views;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using Unity.Injection;
using Unity.Microsoft.DependencyInjection;
using static GrpcService.Greeter;

namespace PrismSampleApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {

        public App()
        {
            ApplicationThemeHelper.ApplicationThemeName = "VS2017Dark";
        }
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

            containerRegistry.RegisterSingleton<IAlfrescoClient, AlfrescoClient>();
            
            IUnityContainer container = containerRegistry.GetContainer();
            

            var services = new ServiceCollection();

            services.AddAlfrescoClient(
            client=> client.BaseAddress = new Uri("http://192.168.30.179:8080/alfresco/api/"),
            alfresco=> 
            {
                alfresco.UserName=ConfigurationManager.AppSettings.Get("Alfresco_UserName");
                alfresco.Password=ConfigurationManager.AppSettings.Get("Alfresco_Password");
            });
            
            services.AddTransient<AbsRefreshTokenHandler>();
            services.AddSingleton<IApiTokenProvider, ApiTokenProvider>();
           
            services.AddSingleton<IRestClient>(new RestClient("http://192.168.30.179:8080/alfresco/api/")
            {
                Authenticator= new HttpBasicAuthenticator("admin", "MtpsF42@Alfresco")
            });
            services.AddHttpClient("zeon", c => c.BaseAddress = new Uri("https://localhost:5001"));
            //  services.AddAutoMapper
            services.AddAutoMapper(options =>
            {
                options.CreateMap<DateTime, Timestamp>()
                .ConvertUsing(s => Timestamp.FromDateTimeOffset(s));

                options.CreateMap<Timestamp,DateTime>()
                .ConvertUsing(s=> s.ToDateTimeOffset().LocalDateTime);

            }, typeof(App).Assembly);
            services
                .AddGrpcClient<GreeterClient>(o =>
            {
                o.Address = new Uri("https://localhost:5002");
            }).AddHttpMessageHandler<AbsRefreshTokenHandler>();

            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<ISignInManager, SignInManager>();

            services.BuildServiceProvider(container);

        }
    }



    public interface ISignInManager
    {
        Task<ClaimsPrincipal> SignInAsync(string userName, string password, int divisionId);
    }

    public class SignInManager : ISignInManager
    {

        private readonly IApiTokenProvider _apiTokenProvider;
        private readonly HttpClient _discoveryClient;
        private readonly IHttpClientFactory _factory;
        public SignInManager(IHttpClientFactory factory, IApiTokenProvider apiTokenProvider)
        {
            _factory = factory;
            _apiTokenProvider = apiTokenProvider;
            _discoveryClient = _factory.CreateClient("zeon");
        }


        public Task<ClaimsPrincipal> SignInAsync(string userName, string password, int divisionId)
        {
            return SignIn(userName, password, divisionId);
        }




        private async Task<ClaimsPrincipal> SignIn(string userName, string password, int divisionId)
        {
            DiscoveryDocumentResponse disco = await _discoveryClient.GetDiscoveryDocumentAsync();

            var tokenResponse = await _discoveryClient.RequestPasswordTokenAsync(new PasswordTokenRequest()
            {
                Address = disco.TokenEndpoint,
                GrantType = "password",


                UserName = userName,
                Password = password,


                Scope = "abscoreapi  offline_access",
                ClientId = "abseROP",
                ClientSecret = "e18ab171-8233-447b-bcb0-1e879613d141",
                Parameters = { { "DivisionId", divisionId.ToString() } }
            });

            if (tokenResponse.IsError)
            {
                return new ClaimsPrincipal(new ClaimsIdentity());
            }

            DateTime expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResponse.ExpiresIn);

            _apiTokenProvider.ReSet(tokenResponse.AccessToken, tokenResponse.RefreshToken, expiresAt.ToString("o", CultureInfo.InvariantCulture));

            ClaimsIdentity newidentity = new ClaimsIdentity("Abs", "Email", ClaimTypes.Role);
            newidentity.AddClaim(new Claim("DivisionId", "1"));
            return new ClaimsPrincipal(newidentity);
        }

    }
}
