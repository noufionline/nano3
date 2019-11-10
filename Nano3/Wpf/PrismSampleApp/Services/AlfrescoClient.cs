using Marvin.StreamExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MimeTypes;
using RestSharp;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PrismSampleApp.Services
{
    public class AlfrescoClient : IAlfrescoClient
    {
        private readonly HttpClient _client;
        private readonly IRestClient _restClient;
        private AlfrescoOptions _options;


        public const string Request = "-default-/public/alfresco/versions/1";

        public AlfrescoClient(HttpClient client, IOptions<AlfrescoOptions> options, IRestClient restClient)
        {
            _client = client;
            _restClient = restClient;
            _options = options.Value;

            var byteArray = Encoding.ASCII.GetBytes($"{_options.UserName}:{_options.Password}");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }


        public async Task<(Guid id, string version)> AttachFileAsync(Guid nodeId, string name, string path, FileOptions options)
        {
            string uri = $"{Request}/nodes/{nodeId}/children";

            var request = new RestRequest(uri, Method.POST);
            request.AddParameter("name", name);
            request.AddParameter("overwrite", options.Overwrite);
            request.AddParameter("cm:title", options.Title);
            request.AddParameter("cm:description", options.Description);

            if (!string.IsNullOrWhiteSpace(options.Comment))
            {
                request.AddParameter("comment", options.Comment);
            }

            if (!string.IsNullOrWhiteSpace(options.RelativePath))
            {
                request.AddParameter("relativePath", options.RelativePath);
            }


            byte[] filedata = File.ReadAllBytes(path);

            request.AddFile("filedata", filedata, name, options.MimeType);


            IRestResponse<Dictionary<string, object>> result = await _restClient.ExecutePostTaskAsync<Dictionary<string, object>>(request);

            return GetId(result);


        }



        private static (Guid id, string version) GetId(IRestResponse<Dictionary<string, object>> result)
        {
            Guid documentId = Guid.Empty;
            var version = string.Empty;
            if (result != null)
            {
                if (result.Data["entry"] is Dictionary<string, object> values)
                {
                    if (Guid.TryParse(values["id"].ToString(), out Guid id))
                    {
                        documentId = id;
                        if (values["properties"] is Dictionary<string, Object> properties)
                        {
                            version = properties["cm:versionLabel"].ToString();
                        }
                    }
                    return (documentId, version);
                }
            }

            return (documentId, version);
        }
        public async Task OpenFileAsync(Guid fileId)
        {
            var requestUri = $"-default-/public/alfresco/versions/1/nodes/{fileId}/content";

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));


            var byteArray = Encoding.ASCII.GetBytes($"{_options.UserName}:{_options.Password}");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));



            using (HttpResponseMessage response =
                await _client.SendAsync(request))
            {
                var contentType = response.Content.Headers.ContentType.MediaType;
                var fileName = response.Content.Headers.ContentDisposition.FileName.Replace("\"", "");
                var exension = MimeTypeMap.GetExtension(contentType);

                using (var stream = await response.Content.ReadAsStreamAsync())
                {

                    response.EnsureSuccessStatusCode();

                    string fileToWriteTo = Path.GetRandomFileName();

                    string tempDirectory = Path.GetTempPath();

                    string directory = Path.Combine(tempDirectory, fileToWriteTo);

                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    string path = Path.Combine(directory, $"{fileName}{exension}");
                    using (FileStream output = new FileStream(path, FileMode.Create))
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        await stream.CopyToAsync(output);
                    }


                    var startInfo = new ProcessStartInfo
                    {
                        UseShellExecute = true,
                        FileName = path,
                        Arguments = @"/r"
                    };

                    Process.Start(startInfo);

                }
            }
        }


       
    }


    public class FileOptions
    {

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the relative path.
        /// </summary>
        /// <value>The relative path.</value>
        public string RelativePath { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="FileOptions"/> is overwrite.
        /// </summary>
        /// <value><c>true</c> if overwrite; otherwise, <c>false</c>.</value>
        public bool Overwrite { get; set; } = true;
        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        public string Comment { get; set; }

        public string MimeType { get; set; } = "application/pdf";
    }
    public static class AlfrescoExtensions
    {
        public static IServiceCollection AddAlfrescoClient(this IServiceCollection service,
            Action<HttpClient> configureClient,
            Action<AlfrescoOptions> configureAlfresco)
        {
            service.AddHttpClient<AlfrescoClient>(configureClient);
            service.AddSingleton<IAlfrescoClient, AlfrescoClient>();
            service.Configure(configureAlfresco);
            return service;
        }

    }


    public class AlfrescoOptions
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Server { get; set; }
    }
}
