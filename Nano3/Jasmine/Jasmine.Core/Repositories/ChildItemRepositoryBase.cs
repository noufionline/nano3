using AspnetWebApi2Helpers.Serialization;
using Jasmine.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace Jasmine.Core.Repositories
{
    public class ChildItemRepositoryBase<T> : RestApiRepositoryBase
        where T : class, IEntity
    {

        public ChildItemRepositoryBase(string parentRoute, string childRoute,IHttpClientFactory factory) : base(factory)
        {
            ParentRoute = parentRoute;
            ChildRoute = childRoute;
        }


        public async Task<(bool success, string errorMessage)> Delete(int parentId, T entity)
        {
            string request = $"{ParentRoute}/{parentId}/{ChildRoute}/{entity.Id}";
            return await DeleteAsync(request);
        }

        public async Task<T> Get(int parentId, int id)
        {
            string request = $"{ParentRoute}/{parentId}/{ChildRoute}/{id}";
            return await ReadAsStreamAsync<T>(request);
        }

        public async Task<List<T>> GetAllAsync(int parentId)
        {
            string request = $"{ParentRoute}/{parentId}/{ChildRoute}";
            return await ReadAllAsStreamAsync<T>(request);
        }

        public async Task<T> Save(int parentId, T entity)
        {
            return await SaveAndReadWithStreamsAsync($"{ParentRoute}/{parentId}/{ChildRoute}", entity);
        }

        public async Task<T> Update(int parentId, T entity)
        {
            return await UpdateAndReadWithStreamsAsync($"{ParentRoute}/{parentId}/{ChildRoute}", entity);
        }

        public string ChildRoute { get; }
        public string ParentRoute { get; }

  
    }
}