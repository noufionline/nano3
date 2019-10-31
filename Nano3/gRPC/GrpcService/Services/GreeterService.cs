using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcService
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        public override async Task GetCustomers(CustomersRequest request, IServerStreamWriter<Customer> responseStream, ServerCallContext context)
        {
            //return base.GetCustomers(request, responseStream, context);
            for (int i = 0; i < 100; i++)
            {
                await responseStream.WriteAsync(new Customer{Id=i+1, Name=$"Name {i}"});
            }
        }
    }
}
