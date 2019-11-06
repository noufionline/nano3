using PrismSampleApp.Mapper;
using GrpcService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrismSampleApp
{
    public interface ICustomerService
    {
        Task<List<CustomerList>> GetAllAsync(string dbName);
        Task<List<Customer>> GetCustomersAsync();
    }
}
