using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jasmine.Abs.Entities;

namespace Jasmine.Abs.Api.Repositories.Contracts
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetAsync(int id);

        Task<T> UpdateAsync(T model);
        Task<T> SaveAsync(T model);


        Task<bool> DeleteAsync(T entity);
    }

    public interface IRepository<T, TList> where T : class, IEntity where TList : class, IEntity
    {
        Task<List<TList>> GetAllAsync();
        Task<T> GetAsync(int id);

        Task<T> UpdateAsync(T model);
        Task<T> SaveAsync(T model);


        Task<bool> DeleteAsync(T entity);
    }
}