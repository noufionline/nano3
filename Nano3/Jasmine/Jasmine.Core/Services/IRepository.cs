using Jasmine.Core.Contracts;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jasmine.Core.Services
{
    public interface IRepository<TModel> where TModel : class, IEntity
    {
        TModel Save(TModel entity);
        TModel Update(TModel entity);
        bool Delete(TModel entity);
        bool Delete(int id);
        TModel Get(int id);

        Task<List<TModel>> GetAllAsync();
    }

    public interface IRepositoryAsync<TModel> where TModel : class, IEntity
    {
        Task<TModel> SaveAsync(TModel entity);
        Task<TModel> UpdateAsync(TModel entity);
        Task<TModel> UpdateAsync(int id, JsonPatchDocument patch);
        Task<(bool success, string errorMessage)> DeleteAsync(TModel entity);
        Task<(bool success, string errorMessage)> DeleteAsync(int id);
        Task<TModel> GetAsync(int id);
        Task<Dictionary<string, List<LookupItem>>> GetLookupItemsAsync(int id);
        Task<List<TModel>> GetAllAsync();

    }

}