using Jasmine.Core.Contracts;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jasmine.Core.Services
{
    //public interface IService<T> where T : class, IEntity
    //{

    //    T Save(T entity);
    //    T Update(T entity);
    //    bool Delete(T entity);
    //    T Get(int id);

    //    Task<List<T>> GetAllAsync();

    //}

    public interface IServiceAsync<T> where T : class, IEntity
    {

        Task<T> SaveAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<(bool success, string errorMessage)> DeleteAsync(T entity);
        Task<(bool success, string errorMessage)> DeleteAsync(int id);
        Task<T> GetAsync(int id);

        Task<List<T>> GetAllAsync();

    }

    public interface IServiceAsync<T, TList> where T : class, IEntity where TList : class, IEntity
    {

        Task<T> SaveAsync(T entity);

        Task<T> UpdateAsync(T entity);
        Task<(bool success, string errorMessage)> DeleteAsync(T entity);
        Task<(bool success, string errorMessage)> DeleteAsync(int id);
        Task<T> GetAsync(int id);

        Task<Dictionary<string, List<LookupItem>>> GetLookupItems(int id);

        Task<List<TList>> GetAllAsync();
    }
}