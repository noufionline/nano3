using GrpcService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace gRpcClient
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllAsync();
    }
}
