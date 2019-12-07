using Jasmine.Core.Aspects;
using Jasmine.Core.Common;
using Jasmine.Core.Contracts;
using Microsoft.AspNetCore.JsonPatch;
using PostSharp.Patterns.Diagnostics;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Jasmine.Core.Repositories
{

    public interface IParentChildRepositoryBaseAsync<T> where T : class, IEntity
    {
        string Request { get; }
        int DivisionId { get; }
        string GetBaseAddress();
        Task<bool> ExistsAsync(string uri);
        Task<T> SaveAsync(int parentId, T entity);
        Task<T> UpdateAsync(int parentId, T entity);
        Task<(bool success, string errorMessage)> DeleteAsync(int parentId, T entity);
        Task<(bool success, string errorMessage)> DeleteAsync(int parentId, int id);
        Task<T> GetAsync(int parentId, int id);
        Task<List<T>> GetAllAsync();
    }

    public class ParentChildRepositoryBaseAsync<T> : RestApiRepositoryBaseAsync<T>, IParentChildRepositoryBaseAsync<T> where T : class, IEntity
    {
        readonly LogSource _logger;

        public ParentChildRepositoryBaseAsync(string request, IHttpClientFactory factory) : base(request, factory)
        {
            _logger = LogSource.Get();
        }


        public Task<bool> ExistsAsync(string uri)
        {
            return CheckExistenceUsingHttpHeadAsync(uri);
        }



        public async Task<T> SaveAsync(int parentId, T entity)
        {
            var requestUri = string.Format(Request, parentId);
            return await SaveAndReadWithStreamsAsync(requestUri, entity);
        }

        public async Task<T> UpdateAsync(int parentId, T entity)
        {
            var requestUri = string.Format(Request, parentId);
            return await UpdateAndReadWithStreamsAsync(requestUri, entity);
        }

        public Task<(bool success, string errorMessage)> DeleteAsync(int parentId, T entity)
        {
            return DeleteAsync(parentId, entity.Id);
        }

        [AutoRetry]
        public async Task<(bool success, string errorMessage)> DeleteAsync(int parentId, int id)
        {
            string request = $"{string.Format(Request, parentId)}/{id}";
            return await DeleteAsync(request);
        }

        public async Task<T> GetAsync(int parentId, int id)
        {
            var requestUri = $"{string.Format(Request, parentId)}/{id}";
            return await ReadAsAsync<T>(requestUri);
        }
    }



    //public class RepositoryBaseAsync<T, TList> : IRepositoryBaseAsync<T, TList> where T : class, IEntity where TList : class, IEntity
    //{
    //    private readonly IHttpClientProvider _provider;
    //    protected MediaTypeFormatter Formatter;
    //    public string Request { get; }

    //    public RepositoryBaseAsync(string request, IHttpClientProvider provider)
    //    {
    //        _provider = provider;
    //        Request = request;
    //        (Formatter, _) = _provider.GetFormatter();
    //    }

    //    public int DivisionId => ClaimsPrincipal.Current.GetDivisionId();
    //    public string GetBaseAddress()
    //    {
    //        string enviorment = ConfigurationManager.AppSettings.Get("Environment");
    //        if (enviorment == "Production")
    //        {
    //            return ConfigurationManager.AppSettings.Get("HttpBaseAddress");

    //        }
    //        return ConfigurationManager.AppSettings.Get("LocalHttpBaseAddress");
    //    }

    //    public async Task<T> SaveAsync(T entity)
    //    {
    //        HttpClient client = await _provider.GetClientAsync();
    //        return await CreateAsync(client, entity, Formatter);

    //    }

    //    public async Task<T> UpdateAsync(T entity)
    //    {
    //        HttpClient client = await _provider.GetClientAsync();
    //        return await UpdateAsync(client, entity, Formatter);

    //    }

    //    public Task<(bool success, string errorMessage)> DeleteAsync(T entity)
    //    {
    //        return DeleteAsync(entity.Id);
    //    }

    //    [AutoRetry]
    //    public async Task<(bool success, string errorMessage)> DeleteAsync(int id)
    //    {
    //        HttpClient client = await _provider.GetClientAsync();
    //        string request = $"{Request}/{id}";
    //        HttpResponseMessage response = await client.DeleteAsync(request);
    //        if (response.IsSuccessStatusCode)
    //        {
    //            return (response.IsSuccessStatusCode, String.Empty);
    //        }
    //        string errorMessage = await response.Content.ReadAsStringAsync();
    //        return (response.IsSuccessStatusCode, errorMessage);
    //    }

    //    public async Task<T> GetAsync(int id)
    //    {
    //        HttpClient client = await _provider.GetClientAsync();
    //        return await GetAsync(client, id, Formatter);
    //    }


    //    public async Task<List<TList>> GetAllAsync()
    //    {
    //        HttpClient client = await _provider.GetClientAsync();
    //        return await GetAllAsync(client, Formatter);
    //    }

    //    public async  Task<Dictionary<string, List<LookupItem>>> GetLookupItemsAsync(int id)
    //    {
    //        HttpClient client = await _provider.GetClientAsync();
    //        string request = $"{Request}/{id}/lookup-items";
    //        HttpResponseMessage response = await client.GetAsync(request);
    //        if (response.StatusCode == HttpStatusCode.NotFound)
    //        {
    //            return null;
    //        }
    //        response.EnsureSuccessStatusCode();
    //        return await response.Content.ReadAsAsync<Dictionary<string, List<LookupItem>>>(new[] { Formatter });
    //    }


    //    //public async Task<bool> IsDuplicatedAsync(int partnerId, string fieldName, string fieldValue)
    //    //{
    //    //    (Formatter, _) = _provider.GetFormatter();
    //    //    HttpClient client = await _provider.GetClientAsync();

    //    //    string request = $"{Request}/{partnerId}/{fieldName}/{fieldValue}";

    //    //    HttpResponseMessage response = await client.GetAsync(request);
    //    //    response.EnsureSuccessStatusCode();
    //    //    bool result = await response.Content.ReadAsAsync<bool>(new[] {Formatter});
    //    //    return result;
    //    //}

    //    public async Task<bool> CheckExistenceUsingHttpHeadAsync(string uri)
    //    {
    //        HttpClient client = await _provider.GetClientAsync();
    //        var message=new HttpRequestMessage(HttpMethod.Head,uri);
    //        var result = await client.SendAsync(message);
    //        return result.IsSuccessStatusCode;
    //    }

    //    [AutoRetry]
    //    private async Task<T> GetAsync(HttpClient client, int id, MediaTypeFormatter formatter)
    //    {
    //        string request = $"{Request}/{id}";
    //        HttpResponseMessage response = await client.GetAsync(request);
    //        response.EnsureSuccessStatusCode();
    //        T result = await response.Content.ReadAsAsync<T>(new[] { formatter });
    //        return result;
    //    }
    //    [AutoRetry]
    //    private async Task<List<TList>> GetAllAsync(HttpClient client, MediaTypeFormatter formatter)
    //    {
    //        HttpResponseMessage response = await client.GetAsync(Request);
    //        response.EnsureSuccessStatusCode();
    //        List<TList> result = await response.Content.ReadAsAsync<List<TList>>(new[] { formatter });
    //        return result;
    //    }
    //    [AutoRetry]
    //    private async Task<T> CreateAsync(HttpClient client, T order, MediaTypeFormatter formatter)
    //    {
    //        HttpResponseMessage response = await client.PostAsync(new Uri(Request, UriKind.Relative), order, formatter);
    //        
    //        response.EnsureSuccessStatusCode();
    //        T result = await response.Content.ReadAsAsync<T>(new[] { formatter });
    //        return result;
    //    }
    //    [AutoRetry]
    //    private async Task<T> UpdateAsync(HttpClient client, T order, MediaTypeFormatter formatter)
    //    {

    //        HttpResponseMessage response = await client.PutAsync(new Uri(Request, UriKind.Relative), order, formatter);

    //        if (response.StatusCode == HttpStatusCode.NotFound)
    //            return null;

    //        response.EnsureSuccessStatusCode();
    //        T result = await response.Content.ReadAsAsync<T>(new[] { formatter });
    //        return result;
    //    }
    //    //[AutoRetry]
    //    //public async Task OpenFileAsync(string url, string fileName)
    //    //{
    //    //    HttpClient client = await _provider.GetClientAsync();

    //    //    using (HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
    //    //    {
    //    //        if (!response.IsSuccessStatusCode)
    //    //        {
    //    //            throw new FileNotFoundException($"Sorry! File {fileName} not found on server");
    //    //        }
    //    //        response.EnsureSuccessStatusCode();

    //    //        using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
    //    //        {
    //    //            string fileToWriteTo = Path.GetRandomFileName();

    //    //            string tempDirectory = Path.GetTempPath();

    //    //            string directory = Path.Combine(tempDirectory, fileToWriteTo);

    //    //            if (!Directory.Exists(directory))
    //    //            {
    //    //                Directory.CreateDirectory(directory);
    //    //            }

    //    //            string path = Path.Combine(directory, fileName);
    //    //            using (Stream streamToWriteTo = File.Open(path, FileMode.Create))
    //    //            {
    //    //                await streamToReadFrom.CopyToAsync(streamToWriteTo);
    //    //            }

    //    //            Process.Start(path, @"/r");
    //    //        }
    //    //    }

    //    //}

    //}


    public class RestApiRepositoryBaseAsync<T> : RestApiRepositoryBase, IRepositoryBaseAsync<T> where T : class, IEntity
    {

        public string Request { get; }

        public RestApiRepositoryBaseAsync(string request, IHttpClientFactory factory) : base(factory)
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




        public async Task<T> SaveAsync(T entity)
        {
            return await SaveAndReadWithStreamsAsync(Request, entity);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            return await UpdateAndReadWithStreamsAsync(Request, entity);
        }
        
        public async Task<T> UpdateAsync(int id, JsonPatchDocument patch)
        {
            var request = $"{Request}/{id}";
            return await UpdateWithPatchAndReadWithStreamsAsync<T>(request, patch);
        }
        public Task<(bool success, string errorMessage)> DeleteAsync(T entity)
        {
            var requestUri = $"{Request}/{entity.Id}";
            return DeleteAsync(requestUri);
        }

        [AutoRetry]
        public async Task<(bool success, string errorMessage)> DeleteAsync(int id)
        {
            string request = $"{Request}/{id}";
            return await DeleteAsync(request);
        }

        public async Task<T> GetAsync(int id)
        {
            string requestUri = $"{Request}/{id}";
            return await ReadAsStreamAsync<T>(requestUri);
        }


        public async Task<List<T>> GetAllAsync()
        {
            return await ReadAllAsStreamAsync<T>(Request);
        }



        [AutoRetry]
        public async Task<TList[]> FilterAsync<TList, TCriteria>(string request, TCriteria criteria)
        {
            return await QueryWithPostAndReadWithStreamsAsync<TList[], TCriteria>(request, criteria);
        }
        [AutoRetry]
        public async Task<TSummary> GetSummary<TSummary, TCriteria>(string request, TCriteria criteria)
        {
            return await QueryWithPostAndReadWithStreamsAsync<TSummary, TCriteria>(request, criteria);
        }


        public async Task<Dictionary<string, List<LookupItem>>> GetLookupItemsAsync(int id)
        {
            string requestUri = $"{Request}/{id}/lookup-items";
            return await ReadAsStreamAsync<Dictionary<string, List<LookupItem>>>(requestUri);
        }
    }
}