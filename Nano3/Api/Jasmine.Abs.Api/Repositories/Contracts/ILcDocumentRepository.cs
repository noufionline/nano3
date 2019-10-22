using Jasmine.Abs.Api.Dto;
using Jasmine.Abs.Api.Dto.AccountReceivables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jasmine.Abs.Api.Repositories.Contracts
{
    public interface ILcDocumentRepository : IRepository<LcDocumentDto, LcDocumentListDto>
    {
        Task<LcDocumentForUpdateDto> GetLcDocumentForUpdateAsync(int id);
        Task<LcDocumentDetailDto> GetLcDocumentDetailById(int id);
        Task<LcDocumentForPrintDto> GetLcDocumentForPrintAsync(int id);
        Task<List<CommercialInvoiceLineForPrintDto>> GetInvoicesAsync(int id);
    }
}
    