using System.Collections.Generic;
using System.Threading.Tasks;


namespace Jasmine.Core.Mvvm
{
    public interface ILookupItemService
    {
        Task<bool> IsDuplicatedAsync(string lookupType, LookupItemModel entity);
        Task<LookupItemModel> SaveAsync(string lookupType, LookupItemModel entity);
        Task<LookupItemModel> UpdateAsync(string lookupType, LookupItemModel entity);
        Task DeleteAsync(string lookupType, LookupItemModel entity);
        Task<LookupItemModel> GetAsync(string lookupType, int id);
        Task<List<LookupItemModel>> GetAllAsync(string lookupType);
        Task<(bool success, string errorMessage)> CheckConcurrency(string lookupType, LookupItemModel entity);
    }
}