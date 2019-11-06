using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using PrismSampleApp.Mapper;
using GrpcService;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using static GrpcService.Greeter;

namespace PrismSampleApp
{
    public class CustomerService : ICustomerService
    {
        private readonly GreeterClient _client;
        private readonly IMapper _mapper;

        public CustomerService(GreeterClient client,IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }
        public async Task<List<CustomerList>> GetAllAsync(string dbName)
        {

            var customers = new List<CustomerList>();

            

            var headers = new Metadata() { { "db", dbName } };
            
            var response = await _client.GetCustomersAsync(new CustomersRequest { Id = 1 }, headers);

            customers.AddRange(_mapper.Map<List<CustomerList>>(response.Customers));

            return customers;
        }


        public async Task<List<Customer>> GetCustomersAsync()
        {
            var customers = new List<Customer>();
            using (var call = _client.GetCustomersAsStreamAsync(new Empty(), new Metadata { { "db", "ABS_CBF2" } }))
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
