using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Jasmine.Abs.Entities.Models.Abs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GrpcService
{
  
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        private readonly AbsClassicContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GreeterService(ILogger<GreeterService> logger,AbsClassicContext context)
        {
            _logger = logger;
            _context = context;
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
            var customers= await _context.Customers.OrderBy(x=> x.CustomerId).Take(10).ToListAsync();
            foreach (var customer in customers)
            {
                await responseStream.WriteAsync(new Customer{Id=customer.CustomerId, Name=$"Name {customer.CustomerName}"});
            }
            ////return base.GetCustomers(request, responseStream, context);
            //for (int i = 0; i < 100; i++)
            //{
            //    await responseStream.WriteAsync(new Customer{Id=i+1, Name=$"Name {i}"});
            //}
        }
    }
}
