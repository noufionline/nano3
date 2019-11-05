using PrismSampleApp.Mapper;
using GrpcService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrismSampleApp
{
    public interface ICustomerService
    {
        Task<List<CustomerList>> GetAllAsync();
        Task<List<Customer>> GetCustomersAsync();
    }
}
