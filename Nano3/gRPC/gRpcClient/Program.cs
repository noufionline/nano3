using Grpc.Core;
using Grpc.Net.Client;
using GrpcService;
using IdentityModel.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using static GrpcService.Greeter;

namespace gRpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {

            IServiceCollection services = new ServiceCollection();

            services.AddTransient<AbsRefreshTokenHandler>();

            services.AddHttpClient<IApiTokenProvider,ApiTokenProvider>
                (client=> client.BaseAddress=new Uri("https://localhost:5001"));

            services
                .AddGrpcClient<GreeterClient>(o =>
            {
               o.Address = new Uri("https://localhost:5002");
            }).AddHttpMessageHandler<AbsRefreshTokenHandler>();

            services.AddTransient<ICustomerService, CustomerService>();

            var b = services.BuildServiceProvider();

            ICustomerService service = b.GetService<ICustomerService>();
            var customers = await service.GetAllAsync();
            foreach (var customer in customers)
            {
                Console.WriteLine($"Customer :{customer.Name}");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

     

        private async static void CreateChannel(IServiceProvider provider, GrpcChannelOptions options)
        {
           var x= provider.GetService<ApiTokenProvider>();
           var token = await x.GetTokenAsync();
          options.HttpClient.SetBearerToken(token);

        }

        
       
    }

    public class AbsRefreshTokenHandler : DelegatingHandler
    {

        private readonly IApiTokenProvider _loginManager;

        public AbsRefreshTokenHandler(IApiTokenProvider loginManager)
        {
            _loginManager = loginManager;
        }

        public AbsRefreshTokenHandler(HttpMessageHandler innerHandler, IApiTokenProvider loginManager) : base(
            innerHandler)
        {
            _loginManager = loginManager;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _loginManager.GetTokenAsync();

            request.SetBearerToken(token);

            return await base.SendAsync(request, cancellationToken);
        }


    }

    public class ApiTokenProvider : IApiTokenProvider
    {
        private readonly HttpClient _client;

        public ApiTokenProvider(HttpClient client)
        {
            _client = client;
        }
        public bool HasToken => !string.IsNullOrWhiteSpace(AccessToken);
        public void ReSet(string accessToken, string refreshToken, string expiresAt)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            ExpiresAt = expiresAt;
        }


        public string AccessToken { get; private set; }
        public string RefreshToken { get; private set; }

        public string ExpiresAt { get; private set; }


        public async Task<string> GetTokenAsync()
        {
            return "newToken";

            bool expired = string.IsNullOrWhiteSpace(ExpiresAt) ||
                           DateTime.Parse(ExpiresAt).AddSeconds(-60).ToUniversalTime() < DateTime.UtcNow;

            //Debug.WriteLine($"Current Time : {DateTime.UtcNow}");
            //Debug.WriteLine($"Expires at : {ExpiresAt}");

            if (expired)
            {
                DiscoveryDocumentResponse disco = await _client.GetDiscoveryDocumentAsync();


                string refreshtoken = RefreshToken;

                var divisionId = Convert.ToInt32(ClaimsPrincipal.Current.FindFirst("divisionId")?.Value);

                if (divisionId == 0)
                    throw new InvalidOperationException("Division cannot be null. Please re login to continue...");


                var response = await _client.RequestRefreshTokenAsync(new RefreshTokenRequest()
                {
                    Address = disco.TokenEndpoint,

                    ClientId = "abseROP",
                    ClientSecret = "e18ab171-8233-447b-bcb0-1e879613d141",
                    Parameters = { { "DivisionId", divisionId.ToString() } },
                    RefreshToken = refreshtoken

                });



                if (!response.IsError)
                {
                    ExpiresAt = (DateTime.UtcNow + TimeSpan.FromSeconds(response.ExpiresIn)).ToString("o", CultureInfo.InvariantCulture);

                    ReSet(response.AccessToken, response.RefreshToken, ExpiresAt);
                }
            }

            return AccessToken;
        }


        public string GetAuthority()
        {
#if DEBUG
            string environment = ConfigurationManager.AppSettings.Get("Environment");

            if (environment == "Production")
            {
                return ConfigurationManager.AppSettings.Get("IdServerEndPoint");
            }

            return ConfigurationManager.AppSettings.Get("LocalIdServerEndPoint");
#else
            return ConfigurationManager.AppSettings["IdServerEndPoint"];
#endif
        }



        public string GetApiEndPoint()
        {
#if DEBUG
            string enviorment = ConfigurationManager.AppSettings.Get("Environment");
            if (enviorment == "Production")
            {
                return ConfigurationManager.AppSettings.Get("HttpBaseAddress");
            }

            return ConfigurationManager.AppSettings.Get("LocalHttpBaseAddress");
#else
            return ConfigurationManager.AppSettings.Get("HttpBaseAddress");
#endif
        }

    }


    public interface IApiTokenProvider
    {
        bool HasToken { get; }
        string AccessToken { get; }
        string RefreshToken { get; }
        string ExpiresAt { get; }
        void ReSet(string accessToken, string refreshToken, string expiresAt);
        Task<string> GetTokenAsync();
        string GetApiEndPoint();
        string GetAuthority();
    }


    public class CustomerService : ICustomerService
    {
        private readonly GreeterClient _client;

        public CustomerService(GreeterClient client)
        {
            _client = client;
        }
        public async Task<List<Customer>> GetAllAsync()
        {
            //var channel = GrpcChannel.ForAddress("https://localhost:5001");
            //var client = new Greeter.GreeterClient(channel);
            var customers = new List<Customer>();

            var token = "My Token";
            var db = "ABS_AUHStore";

            var headers = new Metadata();
         //   headers.Add("Authorization", $"Bearer {token}");
            headers.Add("db", db);

            using (var call = _client.GetCustomers(new CustomersRequest { Id = 1 }, headers))
            {
                await foreach (var customer in call.ResponseStream.ReadAllAsync())
                {
                    customers.Add(customer);
                }
            }

            return customers;
        }
    }

    public interface ICustomerService
    {
        Task<List<Customer>> GetAllAsync();
    }
}
