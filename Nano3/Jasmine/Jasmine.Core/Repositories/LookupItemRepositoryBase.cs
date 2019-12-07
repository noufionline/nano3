using Jasmine.Core.Aspects;
using Jasmine.Core.Contracts;
using PostSharp.Patterns.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Jasmine.Core.Mvvm.LookupItems;
using Marvin.StreamExtensions;
using Newtonsoft.Json;

namespace Jasmine.Core.Repositories
{
 
    public class ErrorInfo
    {
        public string Type { get; set; }
        public string User { get; set; }
        public string ModifiedDate { get; set; }
        public string Name { get; set; }
    }


    public class LookupItemRepository : ILookupItemRepository
    {
        #region Implementation of ILookupItemRepositoryAsync

        public LookupItemRepository(IHttpClientFactory factory)
        {
            Client = factory.CreateClient("abscore");
            Client.Timeout = new TimeSpan(0, 0, 30);
            Client.DefaultRequestHeaders.Clear();
        }

        public HttpClient Client { get; }

        public async Task<LookupItemModel> SaveAsync(string lookupType, LookupItemModel entity)
        {
            var memoryContentStream = new MemoryStream();
            memoryContentStream.SerializeToJsonAndWrite(entity,new UTF8Encoding(),1024,true);
            memoryContentStream.Seek(0, SeekOrigin.Begin);

            using (var request = new HttpRequestMessage(HttpMethod.Post, lookupType))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (var streamContent=new StreamContent(memoryContentStream))
                {
                    request.Content = streamContent;
                    request.Content.Headers.ContentType=new MediaTypeHeaderValue("application/json");

                    using (var response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            var errorMessage = await response.Content.ReadAsStringAsync();
                            var ex=new ApiException(errorMessage,response.StatusCode);
                            throw ex;
                        }
                        var stream = await response.Content.ReadAsStreamAsync();
                        return stream.ReadAndDeserializeFromJson<LookupItemModel>();
                    }
                }
            }
        }


        static ApiException CreateApiException(string errorMessage,HttpStatusCode statusCode)
        {
            
 
            var anonymousErrorObject = 
                new { message = "", ModelState = new Dictionary<string, string[]>() };
 
            // Deserialize:
            var deserializedErrorObject = 
                JsonConvert.DeserializeAnonymousType(errorMessage, anonymousErrorObject);
 
            var ex = new ApiException(errorMessage,statusCode);
 
            if (deserializedErrorObject.ModelState != null)
            {
                var errorList=deserializedErrorObject.ModelState
                    .Select(kvp => string.Join(". ", kvp.Value));
                var errors = errorList as string[] ?? errorList.ToArray();
                for (int i = 0; i < errors.Count(); i++)
                {
                    ex.Data.Add(i, errors.ElementAt(i));
                }
            }
            else
            {
                var error = 
                    JsonConvert.DeserializeObject<Dictionary<string, string[]>>(errorMessage);
                foreach (var kvp in error)
                {
                    foreach (var err in error)
                    {
                        ex.Data.Add(kvp.Key,err);
                    }
                  
                }
            }
            return ex;
        }

        public async Task<LookupItemModel> UpdateAsync(string lookupType, LookupItemModel entity)
        {
            var memoryContentStream = new MemoryStream();
            memoryContentStream.SerializeToJsonAndWrite(entity,new UTF8Encoding(),1024,true);
            memoryContentStream.Seek(0, SeekOrigin.Begin);

            using (var request = new HttpRequestMessage(HttpMethod.Put, lookupType))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (var streamContent=new StreamContent(memoryContentStream))
                {
                    request.Content = streamContent;
                    request.Content.Headers.ContentType=new MediaTypeHeaderValue("application/json");

                    using (var response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            var errorMessage = await response.Content.ReadAsStringAsync();
                            var ex=new ApiException(errorMessage,response.StatusCode);
                            throw ex;
                        }
                        var stream = await response.Content.ReadAsStreamAsync();
                        return stream.ReadAndDeserializeFromJson<LookupItemModel>();
                    }
                }
            }
        }

        public async Task DeleteAsync(string lookupType, LookupItemModel entity)
        {
            string requestUri = $"{lookupType}/{entity.Id}";
            using (var request = new HttpRequestMessage(HttpMethod.Delete, requestUri))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (var response = await Client.SendAsync(request))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        errorMessage=errorMessage?.Replace("\"", string.Empty).Trim();
                        throw new LookupItemIsInUseException(errorMessage);
                    }
                }
            }
        }

        public async Task<LookupItemModel> GetAsync(string lookupType, int id)
        {
            string requestUri = $"{lookupType}/{id}";
            using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

                using (var response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    return stream.ReadAndDeserializeFromJson<LookupItemModel>();
                }
            }
        }

        public async Task<List<LookupItemModel>> GetAllAsync(string lookupType)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, lookupType))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                
                using(var response=await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    response.EnsureSuccessStatusCode();
                    return stream.ReadAndDeserializeFromJson<List<LookupItemModel>>();
                }
            }
        }

        public async Task<object> GetAllOtherLookupItemModels(string lookupType, Type type)
        {
            var requestUri = lookupType;
            using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using (var response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCodeWithAbsExceptions();
                    object result = response.Content.ReadAsAsync(type, CancellationToken.None);
                    return result;
                }
            }
        }

        public async Task<bool> IsDuplicatedAsync(string lookupType, LookupItemModel entity)
        {
            string requestUri = $"{lookupType}/{entity.Id}/{entity.Name}";
            using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using (var response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCodeWithAbsExceptions();
                    return await response.Content.ReadAsAsync<bool>();
                }
            }
        }

        public async Task<(bool success, string errorMessage)> CheckConcurrency(string lookupType, LookupItemModel entity)
        {
            string requestUri=$"{lookupType}/check/concurrency";
            using (var request = new HttpRequestMessage(HttpMethod.Post, requestUri))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

                using (var response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return (true, string.Empty);
                    }

                    var stream = await response.Content.ReadAsStreamAsync();
                    var errorInfo = stream.ReadAndDeserializeFromJson<ErrorInfo>();
                    var errorMessage = string.Format($"Sorry! Cannot update this {errorInfo.Type}. \n{errorInfo.User} has already modified this record at {errorInfo.ModifiedDate}\nNew Value : {errorInfo.Name}");
                    return (response.IsSuccessStatusCode, errorMessage);
                }
            }
        }

        #endregion
    }

    [Serializable]
    public class LookupItemConflictException : ApplicationException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public LookupItemConflictException()
        {
        }

        public LookupItemConflictException(string message) : base(message)
        {
        }

        public LookupItemConflictException(string message, Exception inner) : base(message, inner)
        {
        }

        protected LookupItemConflictException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class ApiException : Exception
    {
      
        public ApiException()
        {
        }

        public ApiException(string message) : base(message)
        {
        }

        public ApiException(string message, Exception inner) : base(message, inner)
        {
        }


        public ApiException(string errorMessage,HttpStatusCode statusCode):base(errorMessage)
        {
            Errors = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(errorMessage);
            StatusCode = statusCode;
        }
        public HttpStatusCode StatusCode { get; }

        public Dictionary<string, string[]> Errors { get; }
        protected ApiException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
   
}