using Microsoft.Extensions.DependencyInjection;
using System;
using Grpc.Core;
using Grpc.Net.Client;
using static GrpcService1.Greeter;
using System.Threading.Tasks;
using GrpcService1;
using Microsoft.Extensions.Http;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;

namespace GrpcClientForNashath
{
    class Program
    {
        static async Task  Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();

            services
                .AddGrpcClient<GreeterClient>(o =>
            {
                o.Address = new Uri("https://192.168.30.208:5001");
            }).ConfigurePrimaryHttpMessageHandler(()=> 
            {
                var clientCertificate = new X509Certificate2(@"C:\Users\Noufal\source\repos\nano3\Nano3\GrpcClientForNashath\grpc.cicononline.com.pfx", "MtpsF42");
                var handler = new HttpClientHandler();
                handler.ClientCertificates.Add(clientCertificate);
                return handler;
            });

            var b = services.BuildServiceProvider();

            var client=b.GetService<GreeterClient>();

             var response = await client.GetCustomersAsync(new CustomerRequest { Id = 1 });
        }
    }
}
