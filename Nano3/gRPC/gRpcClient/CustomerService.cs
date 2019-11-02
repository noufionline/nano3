using Grpc.Core;
using GrpcService;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using static GrpcService.Greeter;

namespace gRpcClient
{
    public class CustomerService : ICustomerService
    {
        private readonly GreeterClient _client;

        public CustomerService(GreeterClient client)
        {
            _client = client;
        }
        public async Task<List<Customer>> GetAllAsync()
        {
            
            var customers = new List<Customer>();

            var db = "ABS_AUHStore";

            var headers = new Metadata(){ {"db",db} };
            
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
}
