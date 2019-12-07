using System.Collections.Generic;
using System.Threading.Tasks;
using Jasmine.Core.Contracts;

namespace Jasmine.Core.Repositories
{
    public interface IParentChildServiceAsync<T> where T : class, IEntity
    {
        
        Task<bool> ExistsAsync(string uri);
        Task<T> SaveAsync(int parentId, T entity);
        Task<T> UpdateAsync(int parentId, T entity);
        Task<(bool success, string errorMessage)> DeleteAsync(int parentId, T entity);
        Task<(bool success, string errorMessage)> DeleteAsync(int parentId, int id);
        Task<T> GetAsync(int parentId, int id);
        Task<List<T>> GetAllAsync();
    }
}