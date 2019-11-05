using MimeTypes;
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
        public AlfrescoClient(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("zeon");
        }
        public async Task OpenFileAsync(string fileId,string userName,string password)
        {
            var requestUri = $"-default-/public/alfresco/versions/1/nodes/{fileId}/content";

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));


             var byteArray = Encoding.ASCII.GetBytes($"{userName}:{password}");
             _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

          

            using (HttpResponseMessage response =
                await _client.SendAsync(request))
            {
                var contentType=response.Content.Headers.ContentType.MediaType;
                var fileName=response.Content.Headers.ContentDisposition.FileName.Replace("\"", "");

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
}
