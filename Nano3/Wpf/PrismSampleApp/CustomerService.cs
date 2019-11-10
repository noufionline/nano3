using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using PrismSampleApp.Mapper;
using GrpcService;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using static GrpcService.Greeter;
using System;
using PrismSampleApp.Dto;

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

        public async Task<List<SteelDeliveryNoteDetailReportData>> GetDeliveryDetailsReportDataAsync(SteelDeliveryNoteDetailReportCriteriaRequest criteria)
        {
            var items = new List<SteelDeliveryNoteDetailReportData>();

            

            var headers = new Metadata() { { "db", criteria.DbName} };
            using (var call = _client.GetDeliveryNoteDetailsReportData(new SteelDeliveryNoteDetailReportCriteriaRequest
            {
                FromDate=Timestamp.FromDateTimeOffset(DateTime.Today),
                ToDate=Timestamp.FromDateTimeOffset(DateTime.Today) ,
                DbName=criteria.DbName
            },headers))
            {
                await foreach (var item in call.ResponseStream.ReadAllAsync())
                {
                    items.Add(_mapper.Map<SteelDeliveryNoteDetailReportData>(item));
                }
            }

            return items;
        }

    }
}
