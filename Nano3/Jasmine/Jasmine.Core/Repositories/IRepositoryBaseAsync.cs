using Jasmine.Core.Contracts;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace Jasmine.Core.Repositories
{
    public interface IRepositoryBaseAsync<T> where T : class, IEntity
    {
        string GetBaseAddress();
        Task<T> SaveAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<(bool success, string errorMessage)> DeleteAsync(T entity);
        Task<(bool success, string errorMessage)> DeleteAsync(int id);
        Task<T> GetAsync(int id);
        Task<List<T>> GetAllAsync();
        //Task OpenFileAsync(string url, string fileName);
        //Task<bool> CheckExistenceUsingHttpHeadAsync(string uri);
        Task<Dictionary<string, List<LookupItem>>> GetLookupItemsAsync(int id);
        Task<TList[]> FilterAsync<TList,TCriteria>(string request, TCriteria criteria);
        Task<TSummary> GetSummary<TSummary,TCriteria>(string request, TCriteria criteria);
    }

    public interface IRepositoryBaseAsync<T, TList> where T : class, IEntity where TList : class, IEntity
    {
        string GetBaseAddress();
        Task<T> SaveAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> UpdateAsync(int id,JsonPatchDocument patch);
        Task<(bool success, string errorMessage)> DeleteAsync(T entity);
        Task<(bool success, string errorMessage)> DeleteAsync(int id);
        Task<T> GetAsync(int id);
        Task<List<TList>> GetAllAsync();
        //Task OpenFileAsync(string url, string fileName);
        Task<Dictionary<string, List<LookupItem>>> GetLookupItemsAsync(int id);

        Task<bool> CheckExistenceUsingHttpHeadAsync(string uri);
    }
}