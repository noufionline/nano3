using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Jasmine.Core.Common;
using Jasmine.Core.Contracts;
using Microsoft.AspNetCore.JsonPatch;

namespace Jasmine.Core.Repositories
{
    public abstract class RestApiRepositoryBaseAsync<T, TList> : RestApiRepositoryBase, IRepositoryBaseAsync<T, TList> where T : class,
        IEntity where TList : class, IEntity
    {
        protected string Request { get; }

        protected RestApiRepositoryBaseAsync(string request, IHttpClientFactory factory) : base(factory)
        {
            Request = request;
        }

        public int DivisionId => ClaimsPrincipal.Current.GetDivisionId();
        public string GetBaseAddress()
        {
            string enviorment = ConfigurationManager.AppSettings.Get("Environment");
            if (enviorment == "Production")
            {
                return ConfigurationManager.AppSettings.Get("HttpBaseAddress");

            }
            return ConfigurationManager.AppSettings.Get("LocalHttpBaseAddress");
        }


        public Task<T> SaveAsync(T entity)
        {
            return SaveAndReadWithStreamsAsync(Request, entity);
        }

        public Task<T> UpdateAsync(T entity)
        {
            return UpdateAndReadWithStreamsAsync(Request, entity);
        }

        public Task<T> UpdateAsync(int id, JsonPatchDocument patch)
        {
            var request =$"{Request}/{id}";
            return UpdateWithPatchAndReadWithStreamsAsync<T>(request, patch);
        }

    

        public Task<(bool success, string errorMessage)> DeleteAsync(T entity)
        {
            return DeleteAsync(entity.Id);
        }

        public async Task<(bool success, string errorMessage)> DeleteAsync(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{Request}/{id}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await Client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return (true, string.Empty);
            }

            //response.EnsureSuccessStatusCodeWithAbsExceptions();
            var errorMessage = await response.Content.ReadAsStringAsync();
            return (false, errorMessage);
        }

        public Task<T> GetAsync(int id)
        {
            return ReadAsStreamAsync<T>($"{Request}/{id}");
        }

        public Task<List<TList>> GetAllAsync()
        {
            return ReadAllAsStreamAsync<TList>(Request);
        }

        public async Task<Dictionary<string, List<LookupItem>>> GetLookupItemsAsync(int id)
        {
            string request = $"{Request}/{id}/lookup-items";
            return await ReadAsStreamAsync<Dictionary<string, List<LookupItem>>>(request);
        }


    }
}