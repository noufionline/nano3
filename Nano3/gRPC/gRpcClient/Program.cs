using Grpc.Core;
using Grpc.Net.Client;
using GrpcService;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static GrpcService.Greeter;

namespace gRpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {

            IServiceCollection services=new ServiceCollection();

            services.AddGrpcClient<GreeterClient>(o =>
            {
                o.Address = new Uri("https://localhost:5001");
            });


            services.AddTransient<ICustomerService,CustomerService>();

            var b=services.BuildServiceProvider();


            ICustomerService service =b.GetService<ICustomerService>();
            var customers = await service.GetAllAsync();
            foreach (var customer in customers)
            {
                Console.WriteLine($"Customer :{customer.Name}");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }




    }


    public class CustomerService:ICustomerService
    {
        private readonly Greeter.GreeterClient _client;

        public CustomerService(Greeter.GreeterClient client)
        {
            _client = client;
        }
        public async Task<List<Customer>> GetAllAsync()
        {
            //var channel = GrpcChannel.ForAddress("https://localhost:5001");
            //var client = new Greeter.GreeterClient(channel);
            var customers=new List<Customer>();

            var token ="My Token";
            var db="ABS_AUHStore";

             var headers = new Metadata();
             headers.Add("Authorization", $"Bearer {token}");
            headers.Add("db",db);

            using (var call = _client.GetCustomers(new CustomersRequest { Id = 1 },headers))
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
