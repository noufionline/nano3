using AutoMapper;
using Grpc.Core;
using Grpc.Net.Client;
using IdentityModel.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
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
            services.AddSingleton<IApiTokenProvider, ApiTokenProvider>();

            services.AddHttpClient("zeon", c => c.BaseAddress = new Uri("https://localhost:5001"));
          //  services.AddAutoMapper
            services.AddAutoMapper(typeof(Program).Assembly);
            services
                .AddGrpcClient<GreeterClient>(o =>
            {
                o.Address = new Uri("https://localhost:5002");
            }).AddHttpMessageHandler<AbsRefreshTokenHandler>();

            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<ISignInManager, SignInManager>();

            var b = services.BuildServiceProvider();


            var sm = b.GetService<ISignInManager>();

            var result = await sm.SignInAsync("noufal@cicon.net", "MtpsF42@", 1);


            ICustomerService service = b.GetService<ICustomerService>();
            try
            {
                var customers = await service.GetAllAsync();
                foreach (var customer in customers)
                {
                    Console.WriteLine($"Customer :{customer.Name} - Salary {customer.Salary}" );
                    Console.WriteLine("==========Projects=================================");
                    foreach (var project in customer.Projects)
                    {
                        Console.WriteLine($"        Project :{project.Name}");
                    }
                }
            }
            catch (RpcException e)
            {
                Console.WriteLine(e.Status.Detail);
                Console.WriteLine(e.Status.StatusCode);
             
                // Want to take specific action based on specific error?
                if (e.Status.StatusCode == Grpc.Core.StatusCode.Unauthenticated)
                {
                    // do your thing
                }
            }


            



            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }



        private async static void CreateChannel(IServiceProvider provider, GrpcChannelOptions options)
        {
            var x = provider.GetService<ApiTokenProvider>();
            var token = await x.GetTokenAsync();
            options.HttpClient.SetBearerToken(token);

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
