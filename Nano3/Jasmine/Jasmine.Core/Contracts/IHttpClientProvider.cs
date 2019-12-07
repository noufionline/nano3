using AspnetWebApi2Helpers.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Jasmine.Core.Contracts
{
    //public interface IHttpClientProvider
    //{
    //    Task<HttpClient> GetClientAsync();
    //    (MediaTypeFormatter Formatter, string AcceptHeader) GetFormatter();

    //    IEnumerable<string> GetIpAddresses();
    //}


    public interface ILookupItemAuthorizationProvider
    {
        string[] GetRoutes();
        string GetResource(string route);
        void AddResources(params KeyValuePair<string,string>[] routes);
    }

    public class LookupItemAuthorizationProvider : ILookupItemAuthorizationProvider
    {
        private readonly Dictionary<string, string> _routes;
       
        public LookupItemAuthorizationProvider()
        {
            _routes = new Dictionary<string, string>();
        }

        public string GetResource(string route)
        {
            if (_routes.TryGetValue(route, out string resource))
            {
                return resource;
            }
            return null;
        }

        public void AddResources(params KeyValuePair<string,string>[] items)
        {
            foreach (var item in items)
            {
                if(string.IsNullOrWhiteSpace(item.Value)) continue;

                if (!_routes.ContainsKey(item.Key))
                {
                    _routes.Add(item.Key,item.Value);
                }
            }
        }

        public string[] GetRoutes()
        {
            return _routes.Keys.ToArray();
        }
    }

    ////public abstract class BasicHttpClientProvider : IHttpClientProvider
    ////{
    ////    private readonly IHttpClientFactory _factory;
    ////    

    ////    protected BasicHttpClientProvider(IHttpClientFactory factory)
    ////    {
    ////        _factory = factory;
    ////    }

    ////    public virtual Task<HttpClient> GetClientAsync() =>Task.FromResult(_factory.CreateClient("abscore"));

    ////    public (MediaTypeFormatter Formatter, string AcceptHeader) GetFormatter()
    ////    {
    ////        JsonMediaTypeFormatter jsonFormatter = new JsonMediaTypeFormatter();
    ////        jsonFormatter.JsonPreserveReferences();
    ////        return (jsonFormatter, "application/json");
    ////    }

    ////    public IEnumerable<string> GetIpAddresses()
    ////    {
    ////        var host = Dns.GetHostEntry(Dns.GetHostName());

    ////        return (from ip in host.AddressList
    ////            where ip.AddressFamily == AddressFamily.InterNetwork
    ////            select ip.ToString()).ToList();
    ////    }
    ////}

    //public class TokenBasedHttpClientProvider : BasicHttpClientProvider
    //{
    //    private readonly IHttpClientFactory _factory;
    //    
    //    public TokenBasedHttpClientProvider(IHttpClientFactory factory) :
    //        base(factory)
    //    {
    //        _factory = factory;
    //        
    //    }

    // 
    //    public override  Task<HttpClient> GetClientAsync()
    //    {   
    //        return Task.FromResult(_factory.CreateClient("abscore"));
    //    }

    //   
    //}

    [Serializable]
    public class NotAuthenticatedException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public NotAuthenticatedException()
        {
        }

        public NotAuthenticatedException(string message) : base(message)
        {
        }

        public NotAuthenticatedException(string message, Exception inner) : base(message, inner)
        {
        }

        protected NotAuthenticatedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}