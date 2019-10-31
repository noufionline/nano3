using Grpc.Core;
using Grpc.Net.Client;
using GrpcService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace gRpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ICustomerService service = new CustomerService();
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
        public async Task<List<Customer>> GetAllAsync()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Greeter.GreeterClient(channel);
            var customers=new List<Customer>();

            using (var call = client.GetCustomers(new CustomersRequest { Id = 1 }))
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
