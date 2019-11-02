using gRpcClient.Mapper;
using GrpcService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace gRpcClient
{
    public interface ICustomerService
    {
        Task<List<CustomerList>> GetAllAsync();
        Task<List<Customer>> GetCustomersAsync();
    }
}
