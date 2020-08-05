using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dapper;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcService.Contracts;
using GrpcService.Dto;
using GrpcService.Reports;
using Jasmine.Abs.Entities.Models.Abs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SqlConnection = System.Data.SqlClient.SqlConnection;

namespace GrpcService
{
    [Authorize]
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        private readonly IAbsConnectionStringProvider _connectionStringProvider;

        private readonly AbsClassicContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authz;

        public GreeterService(ILogger<GreeterService> logger,
          IAbsConnectionStringProvider connectionStringProvider,
            AbsClassicContext context, IMapper mapper, IAuthorizationService authz)
        {
            _logger = logger;
            _connectionStringProvider = connectionStringProvider;

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
                var status = new Status(StatusCode.Unauthenticated, "Not Authorized");
                throw new RpcException(status);
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
                List<Customer> grpcCustomers = _mapper.Map<List<Customer>>(customers);

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


        public override async Task GetDeliveryNoteDetailsReportData(SteelDeliveryNoteDetailReportCriteriaRequest request, IServerStreamWriter<SteelDeliveryNoteDetailReportDataResponse> responseStream, ServerCallContext context)
        {
            var criteria = _mapper.Map<SteelDeliveryNoteDetailReportCriteriaDto>(request);

            var connectionString = _connectionStringProvider.ConnectionString;
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Open();

                    var data = await con
                        .QueryAsync<SteelDeliveryNoteDetailReportData>("[Reports].[GetDeliveryNoteDetails_SalesAndServices]", criteria, commandType: System.Data.CommandType.StoredProcedure)
                        .ConfigureAwait(false);

                    var items = _mapper.Map<List<SteelDeliveryNoteDetailReportDataResponse>>(data);

                    foreach (var item in items)
                    {
                        await responseStream.WriteAsync(item);
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Database is not specified");
            }

        }


        public override async Task DownloadReportFile(ReportRequest request, IServerStreamWriter<DataChunk> responseStream, ServerCallContext context)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Test.pdf");
            var fileStream = await File.ReadAllBytesAsync(path);
            var data = new DataChunk();
            data.Data = ByteString.CopyFrom(fileStream);
            await responseStream.WriteAsync(data);


            //   var rpt = new TestReport();
            //   rpt.DataSource = Reports.Customer.GetCustomers();

            //   await new TaskFactory().StartNew(async () =>
            //{
            //    rpt.CreateDocument();
            //    using (MemoryStream ms = new MemoryStream())
            //    {

            //        rpt.ExportToPdf(ms);

            //        byte[] exportedFileBytes = ms.ToArray();
            //        var data = new DataChunk();
            //        data.Data = ByteString.CopyFrom(exportedFileBytes);
            //        await responseStream.WriteAsync(data);
            //    }
            //});


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
}
