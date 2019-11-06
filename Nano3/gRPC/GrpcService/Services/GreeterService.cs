using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Jasmine.Abs.Entities.Models.Abs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static GrpcService.AutoMapper.CustomerProfile;

namespace GrpcService
{
    [Authorize]
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        private readonly AbsClassicContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authz;

        public GreeterService(ILogger<GreeterService> logger,
            AbsClassicContext context, IMapper mapper, IAuthorizationService authz)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _authz = authz;
        }

        [AllowAnonymous]
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }


        public override async Task<CustomersResponse> GetCustomers(CustomersRequest request, ServerCallContext context)
        {
            var user = context.GetHttpContext().User;
            AuthorizationResult result = await _authz.AuthorizeAsync(user, "CreateDebtorStatement");

            if (!result.Succeeded)
            {
                throw new RpcException(Status.DefaultCancelled);
            }

            var customers = await _context.Customers
                .Where(x => x.PartnerId != null && x.Projects.Count > 0)
                .OrderBy(x => x.CustomerId)
                .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var customersWithoutPartners = await _context
                .Customers.Where(x => x.PartnerId == null)
                .OrderBy(x => x.CustomerId)
                .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            customers.AddRange(customersWithoutPartners);

            foreach (var customer in customers)
            {
                customer.CreatedDate = DateTime.Today;
                customer.Salary = 10000;
            }

            var response = new CustomersResponse();

            try
            {
                var grpcCustomers = _mapper.Map<List<Customer>>(customers);

                response.Customers.AddRange(grpcCustomers);
            }
            catch (Exception ex)
            {
                throw;
            }



            return response;

        }

        public override async Task GetCustomersAsStreamAsync(Empty request, IServerStreamWriter<Customer> responseStream, ServerCallContext context)
        {


            var user = context.GetHttpContext().User;
            AuthorizationResult result = await _authz.AuthorizeAsync(user, "CreateDebtorStatement");

            if (!result.Succeeded)
            {
                throw new RpcException(Status.DefaultCancelled);
            }


            var customers = await _context.Customers
            .OrderBy(x => x.CustomerId)
            .Take(10).ProjectTo<Customer>(_mapper.ConfigurationProvider)
            .ToListAsync();

            foreach (var customer in customers)
            {
                await responseStream.WriteAsync(customer);
            }
        }




        //public override async Task GetCustomersAsStreamAsync(IServerStreamWriter<Customer> responseStream, ServerCallContext context)
        //{
        //    var customers = await _context.Customers
        //        .OrderBy(x => x.CustomerId)
        //        .Take(10).ProjectTo<Customer>(_mapper.ConfigurationProvider)
        //        .ToListAsync();

        //    foreach (var customer in customers)
        //    {
        //       await responseStream.WriteAsync(customer);
        //    }
        //}
        //public override async Task GetCustomers(CustomersRequest request, IServerStreamWriter<Customer> responseStream, ServerCallContext context)
        //{
        //    var customers= await _context.Customers.OrderBy(x=> x.CustomerId).Take(10).ToListAsync();
        //    foreach (var customer in customers)
        //    {
        //        await responseStream.WriteAsync(new Customer{Id=customer.CustomerId, Name=$"Name {customer.CustomerName}"});
        //    }
        //    ////return base.GetCustomers(request, responseStream, context);
        //    //for (int i = 0; i < 100; i++)
        //    //{
        //    //    await responseStream.WriteAsync(new Customer{Id=i+1, Name=$"Name {i}"});
        //    //}
        //}

    }

    public partial class DecimalValue
    {
        private const decimal NanoFactor = 1_000_000_000;

        public DecimalValue(long units, int nanos)
        {
            Units = units;
            Nanos = nanos;
        }

        public static implicit operator decimal(DecimalValue decimalValue) => decimalValue.ToDecimal();

        public static implicit operator DecimalValue(decimal value) => FromDecimal(value);

        public decimal ToDecimal()
        {
            return Units + Nanos / NanoFactor;
        }

        public static DecimalValue FromDecimal(decimal value)
        {
            var units = decimal.ToInt64(value);
            var nanos = decimal.ToInt32((value - units) * NanoFactor);
            return new DecimalValue(units, nanos);
        }
    }
}
