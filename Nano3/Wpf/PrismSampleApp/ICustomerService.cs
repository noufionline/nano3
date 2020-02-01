using PrismSampleApp.Mapper;
using GrpcService;
using System.Collections.Generic;
using System.Threading.Tasks;
using PrismSampleApp.Dto;

namespace PrismSampleApp
{
    public interface ICustomerService
    {
        Task<List<CustomerList>> GetAllAsync(string dbName);
        Task<List<Customer>> GetCustomersAsync();
        Task<List<SteelDeliveryNoteDetailReportData>> GetDeliveryDetailsReportDataAsync(SteelDeliveryNoteDetailReportCriteriaRequest criteria);
        Task<byte[]> GetFileAsync();
    }
}
