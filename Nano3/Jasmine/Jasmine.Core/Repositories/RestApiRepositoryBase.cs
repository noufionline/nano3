using Marvin.StreamExtensions;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Jasmine.Core.Repositories
{
    public abstract class RestApiRepositoryBase
    {
        readonly IHttpClientFactory _factory;
        protected RestApiRepositoryBase(IHttpClientFactory factory)
        {
            _factory = factory;
            Client = factory.CreateClient("abscore");
            Client.Timeout = new TimeSpan(0, 0, 30);
            Client.DefaultRequestHeaders.Clear();
        }


        protected HttpClient Client { get; set; }

        public async Task<List<T>> ReadAllAsStreamAsync<T>(string requestUri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            using (var response =
                await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    response.EnsureSuccessStatusCode();
                    return stream.ReadAndDeserializeFromJson<List<T>>();
                }
            }
        }

        public async Task<T> ReadAsStreamAsync<T>(string requestUri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            using (var response =
                await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    if (!response.IsSuccessStatusCode && response.StatusCode == (HttpStatusCode)422)
                    {
                        var errors = await GetValidationErrorsAsync(response.Content);
                        throw new EntityValidationException(errors);
                    }
                    response.EnsureSuccessStatusCode();
                    return stream.ReadAndDeserializeFromJson<T>();
                }
            }
        }

        public async Task<T> ReadAsAsync<T>(string requestUri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            using (var response =
                await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                var success = await response.Content.ReadAsAsync<T>();
                response.EnsureSuccessStatusCode();
                return success;
            }
        }


        public async Task<string> ReadAsStringAsync(string requestUri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            using (var response =
                await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                var result = await response.Content.ReadAsStringAsync();
                result = result?.Replace("\"", string.Empty).Trim();
                response.EnsureSuccessStatusCode();
                return result;
            }
        }

        public async Task PostAsStreamsAsync<T>(string requestUri, T entity)
        {
            using (var memoryContentStream = new MemoryStream())
            {
                memoryContentStream.SerializeToJsonAndWrite(entity, new UTF8Encoding(), 1024, true);
                memoryContentStream.Seek(0, SeekOrigin.Begin);

                using (var request = new HttpRequestMessage(HttpMethod.Post, requestUri))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    using (var streamContent = new StreamContent(memoryContentStream))
                    {
                        request.Content = streamContent;
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        using (var response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                        {
                            response.EnsureSuccessStatusCodeWithAbsExceptions();
                        }
                    }
                }
            }
        }

        public async Task PutAsStreamsAsync<T>(string requestUri, T entity)
        {
            using (var memoryContentStream = new MemoryStream())
            {
                memoryContentStream.SerializeToJsonAndWrite(entity, new UTF8Encoding(), 1024, true);
                memoryContentStream.Seek(0, SeekOrigin.Begin);

                using (var request = new HttpRequestMessage(HttpMethod.Put, requestUri))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    using (var streamContent = new StreamContent(memoryContentStream))
                    {
                        request.Content = streamContent;
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        using (var response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                        {
                            response.EnsureSuccessStatusCodeWithAbsExceptions();
                        }
                    }
                }
            }
        }

        public async Task<T> SaveAndReadWithStreamsAsync<T>(string requestUri, T entity)
        {
            using (var memoryContentStream = new MemoryStream())
            {
                memoryContentStream.SerializeToJsonAndWrite(entity, new UTF8Encoding(), 1024, true);
                memoryContentStream.Seek(0, SeekOrigin.Begin);

                using (var request = new HttpRequestMessage(HttpMethod.Post, requestUri))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    using (var streamContent = new StreamContent(memoryContentStream))
                    {
                        request.Content = streamContent;
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        using (var response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                        {
                            if (!response.IsSuccessStatusCode && response.StatusCode == (HttpStatusCode)422)
                            {
                                var errors = await GetValidationErrorsAsync(response.Content);
                                throw new EntityValidationException(errors);
                            }
                            response.EnsureSuccessStatusCodeWithAbsExceptions();
                            using (var stream = await response.Content.ReadAsStreamAsync())
                            {
                                return stream.ReadAndDeserializeFromJson<T>();
                            }
                        }
                    }
                }
            }
        }

        protected async Task<Dictionary<string, string[]>> GetValidationErrorsAsync(HttpContent content)
        {
            var httpErrorObject = await content.ReadAsStringAsync();
            // Deserialize:
            var deserializedErrorObject =
                JsonConvert.DeserializeObject<Dictionary<string, string[]>>(httpErrorObject);

            return deserializedErrorObject;
        }

        public async Task<TResult> QueryWithPostAndReadWithStreamsAsync<TResult, T>(string requestUri, T entity)
        {
            using (var memoryContentStream = new MemoryStream())
            {
                memoryContentStream.SerializeToJsonAndWrite(entity, new UTF8Encoding(), 1024, true);
                memoryContentStream.Seek(0, SeekOrigin.Begin);

                using (var request = new HttpRequestMessage(HttpMethod.Post, requestUri))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    using (var streamContent = new StreamContent(memoryContentStream))
                    {
                        request.Content = streamContent;
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        using (var response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                        {
                            response.EnsureSuccessStatusCodeWithAbsExceptions();
                            using (var stream = await response.Content.ReadAsStreamAsync())
                            {
                                return stream.ReadAndDeserializeFromJson<TResult>();
                            }
                        }
                    }
                }
            }
        }
        public async Task<TResult> QueryWithPutAndReadWithStreamsAsync<TResult, T>(string requestUri, T entity)
        {
            using (var memoryContentStream = new MemoryStream())
            {
                memoryContentStream.SerializeToJsonAndWrite(entity, new UTF8Encoding(), 1024, true);
                memoryContentStream.Seek(0, SeekOrigin.Begin);

                using (var request = new HttpRequestMessage(HttpMethod.Put, requestUri))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    using (var streamContent = new StreamContent(memoryContentStream))
                    {
                        request.Content = streamContent;
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        using (var response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                        {
                            response.EnsureSuccessStatusCodeWithAbsExceptions();
                            using (var stream = await response.Content.ReadAsStreamAsync())
                            {
                                return stream.ReadAndDeserializeFromJson<TResult>();
                            }
                        }
                    }
                }
            }
        }
        public async Task<TResult> QueryWithDeleteAndReadWithStreamsAsync<TResult>(string requestUri)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, requestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (var response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCodeWithAbsExceptions();
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    return stream.ReadAndDeserializeFromJson<TResult>();
                }
            }
        }
        public async Task<bool> PostAndReturnStatusWithStreamAsync<T>(string requestUri, T entity)
        {
            using (var memoryContentStream = new MemoryStream())
            {
                memoryContentStream.SerializeToJsonAndWrite(entity, new UTF8Encoding(), 1024, true);
                memoryContentStream.Seek(0, SeekOrigin.Begin);

                using (var request = new HttpRequestMessage(HttpMethod.Post, requestUri))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    using (var streamContent = new StreamContent(memoryContentStream))
                    {
                        request.Content = streamContent;
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        using (var response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                        {
                            response.EnsureSuccessStatusCodeWithAbsExceptions();
                            var success = Convert.ToBoolean(await response.Content.ReadAsStringAsync());
                            return success;
                        }
                    }
                }
            }
        }


        public async Task<bool> PostAndReturnStatusWithStreamAsync<T>(string requestUri, T entity, TimeSpan timeout)
        {
            using (var memoryContentStream = new MemoryStream())
            {
                memoryContentStream.SerializeToJsonAndWrite(entity, new UTF8Encoding(), 1024, true);
                memoryContentStream.Seek(0, SeekOrigin.Begin);

                using (var request = new HttpRequestMessage(HttpMethod.Post, requestUri))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    using (var streamContent = new StreamContent(memoryContentStream))
                    {
                        request.Content = streamContent;
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        var client = _factory.CreateClient("abscore");
                        client.Timeout = timeout;
                        using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                        {
                            response.EnsureSuccessStatusCodeWithAbsExceptions();
                            var success = Convert.ToBoolean(await response.Content.ReadAsStringAsync());
                            return success;
                        }
                    }
                }
            }
        }

        public async Task<bool> PutAndReturnStatusWithStreamAsync<T>(string requestUri, T entity)
        {
            using (var memoryContentStream = new MemoryStream())
            {
                memoryContentStream.SerializeToJsonAndWrite(entity, new UTF8Encoding(), 1024, true);
                memoryContentStream.Seek(0, SeekOrigin.Begin);

                using (var request = new HttpRequestMessage(HttpMethod.Put, requestUri))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    using (var streamContent = new StreamContent(memoryContentStream))
                    {
                        request.Content = streamContent;
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        using (var response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                        {
                            response.EnsureSuccessStatusCodeWithAbsExceptions();
                            var success = Convert.ToBoolean(await response.Content.ReadAsStringAsync());
                            return success;
                        }
                    }
                }
            }
        }

        public async Task<bool> IsExistsAsync(string requestUri)
        {
            var request = new HttpRequestMessage(HttpMethod.Head, requestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            using (var response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCodeWithAbsExceptions();
                var exists = await response.Content.ReadAsAsync<bool>();
                return exists;
            }
        }

        public async Task<T> UpdateAndReadWithStreamsAsync<T>(string requestUri, T entity)
        {
            using (var memoryContentStream = new MemoryStream())
            {
                memoryContentStream.SerializeToJsonAndWrite(entity, new UTF8Encoding(), 1024, true);
                memoryContentStream.Seek(0, SeekOrigin.Begin);

                using (var request = new HttpRequestMessage(HttpMethod.Put, requestUri))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    using (var streamContent = new StreamContent(memoryContentStream))
                    {
                        request.Content = streamContent;
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        using (var response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                        {
                            if (!response.IsSuccessStatusCode && response.StatusCode == (HttpStatusCode)422)
                            {
                                var errors = await GetValidationErrorsAsync(response.Content);
                                throw new EntityValidationException(errors);
                            }
                            response.EnsureSuccessStatusCodeWithAbsExceptions();
                            using (var stream = await response.Content.ReadAsStreamAsync())
                            {
                                return stream.ReadAndDeserializeFromJson<T>();
                            }
                        }
                    }
                }
            }
        }


        public async Task<T> UpdateWithPatchAndReadWithStreamsAsync<T>(string requestUri,JsonPatchDocument patch)
        {
             var serializedChangeSet = JsonConvert.SerializeObject(patch);

            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, requestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(serializedChangeSet);
            request.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/json-patch+json");


            using (var response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                if (!response.IsSuccessStatusCode && response.StatusCode == (HttpStatusCode)422)
                {
                    var errors = await GetValidationErrorsAsync(response.Content);
                    throw new EntityValidationException(errors);
                }
                response.EnsureSuccessStatusCodeWithAbsExceptions();
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    return stream.ReadAndDeserializeFromJson<T>();
                }
            }
        }

        public async Task<(bool success, string errorMessage)> DeleteAsync(string requestUri)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, requestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCodeWithAbsExceptions();
            if (response.IsSuccessStatusCode) return (true, string.Empty);

            var errorMessage = await response.Content.ReadAsStringAsync();
            return (false, errorMessage);
        }


        public async Task<bool> CheckExistenceUsingHttpHeadAsync(string uri)
        {
            var message = new HttpRequestMessage(HttpMethod.Head, uri);
            var result = await Client.SendAsync(message);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> CheckExistenceUsingHttpGetAsync(string uri)
        {
            var message = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await Client.SendAsync(message);
            response.EnsureSuccessStatusCodeWithAbsExceptions();
            return await response.Content.ReadAsAsync<bool>();
        }
    }


    public static class HttpRestApiExtensions
    {
        public static void EnsureSuccessStatusCodeWithAbsExceptions(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Forbidden:
                        throw new HttpRequestException("You do not have access to this resource.");
                    case HttpStatusCode.NotFound:
                        throw new HttpRequestException("Invalid Address");
                    // trigger a login flow ( Show the login Form)
                    case HttpStatusCode.Unauthorized:
                        throw new HttpRequestException("You are not Authorized!!!");
                    default:
                        response.EnsureSuccessStatusCode();
                        break;
                }
        }
    }


    [Serializable]
    public class EntityValidationException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //


        public Dictionary<string, string[]> Errors { get; }
        public EntityValidationException(Dictionary<string, string[]> errors)
        {
            Errors = errors;
        }

        public EntityValidationException(string message, Dictionary<string, string[]> errors) : base(message)
        {
            Errors = errors;
        }

        public EntityValidationException(string message, Exception inner, Dictionary<string, string[]> errors) : base(message, inner)
        {
            Errors = errors;
        }

        protected EntityValidationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}