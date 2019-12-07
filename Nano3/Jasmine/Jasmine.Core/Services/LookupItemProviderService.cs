using Jasmine.Core.Contracts;
using PostSharp.Patterns.Caching;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jasmine.Core.Services
{
    //public class LookupItemProviderService : ILookupItemProviderService
    //{
    //    private readonly ILookupItemProviderRepository _repository;


    //    public LookupItemProviderService(ILookupItemProviderRepository repository)
    //    {
    //        _repository = repository;
    //    }


    //    //public async Task<List<LookupItem>> GetLookupItemsAsync(string route, bool invalidateCache = false)
    //    //{
    //    //    if (invalidateCache)
    //    //    {
    //    //        await CachingServices.Invalidation.InvalidateAsync(_repository.GetLookupItemsAsync, route);
    //    //    }

    //    //    return await _repository.GetLookupItemsAsync(route);
    //    //}
    //    ////public async Task<List<T>> GetLookupItemsAsync<T>(string route, bool invalidateCache = false) where T : class, ILookupItem
    //    ////{
    //    ////    if (invalidateCache)
    //    ////    {
    //    ////        await CachingServices.Invalidation.InvalidateAsync(_repository.GetLookupItemsAsync<T>, route);
    //    ////    }

    //    ////    return await _repository.GetLookupItemsAsync<T>(route);
    //    ////}

    //    //public async Task<List<LookupItem>> GetLookupItemsAsync(string route, int id, bool invalidateCache = false)
    //    //{
    //    //    route = $"{route}/{id}";

    //    //    if (invalidateCache)
    //    //    {
    //    //        await CachingServices.Invalidation.InvalidateAsync(_repository.GetLookupItemsByIdAsync, route);
    //    //    }

    //    //    return await _repository.GetLookupItemsByIdAsync(route);
    //    //}

    //    ////public async Task<List<T>> GetLookupItemsAsync<T>(string route, int id, bool invalidateCache = false) where T : class, ILookupItem
    //    ////{

    //    ////    route = $"{route}/{id}";

    //    ////    if (invalidateCache)
    //    ////    {
    //    ////        await CachingServices.Invalidation.InvalidateAsync(_repository.GetLookupItemsByIdAsync<T>, route);
    //    ////    }

    //    ////    return await _repository.GetLookupItemsByIdAsync<T>(route);
    //    ////}

    //    //public async Task<List<T>> GetOtherLookupItemsAsync<T>(string route, bool invalidateCache = false) where T : class
    //    //{
    //    //    if (invalidateCache)
    //    //    {
    //    //        await CachingServices.Invalidation.InvalidateAsync(_repository.GetOtherLookupItemsAsync<T>, route);
    //    //    }

    //    //    return await _repository.GetOtherLookupItemsAsync<T>(route);
    //    //}

    //    //public async Task<List<T>> GetOtherLookupItemsAsync<T>(string route, int id, bool invalidateCache = false) where T : class
    //    //{
    //    //    route = $"{route}/{id}";
    //    //    if (invalidateCache)
    //    //    {
    //    //        await CachingServices.Invalidation.InvalidateAsync(_repository.GetOtherLookupItemsAsync<T>, route);
    //    //    }

    //    //    return await _repository.GetOtherLookupItemsAsync<T>(route);
    //    //}
    //}
}