using Jasmine.Core.Contracts;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Jasmine.Core.Alfresco
{
    /// <summary>
    /// Class FileOptions.
    /// </summary>
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

        public string MimeType{get;set;}="application/pdf";
    }

    /// <summary>
    /// Class AlfrescoClient.
    /// </summary>
    public class AlfrescoClient : IAlfrescoClient
    {
        private readonly string _username;
        private readonly string _password;

        /// <summary>
        /// The request
        /// </summary>
        public const string Request = "alfresco/api/-default-/public/alfresco/versions/1";

        /// <summary>
        /// Gets the base address.
        /// </summary>
        /// <value>The base address.</value>
        public string BaseAddress { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlfrescoClient"/> class.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public AlfrescoClient(string baseAddress)
        {
            _username = ClaimsPrincipal.Current.FindFirst("AlfrescoUserName").Value;
            _password = ClaimsPrincipal.Current.FindFirst("AlfrescoPassword").Value;
            BaseAddress = baseAddress;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlfrescoClient"/> class.
        /// </summary>
        public AlfrescoClient() : this(ConfigurationManager.AppSettings.Get("AlfrescoDomain")
           )
        {

        }

        /// <summary>
        /// attach file as an asynchronous operation.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="path">The path.</param>
        /// <param name="options"></param>
        /// <returns>Task&lt;Guid&gt;.</returns>
        public async Task<(Guid id, string version)> AttachFileAsync(Guid nodeId, string name, string path,FileOptions options)
        {
            string uri =
                $"{BaseAddress}/{Request}/nodes/{nodeId}/children";
            var client =
                new RestClient(uri)
                {
                    Authenticator = new HttpBasicAuthenticator(_username, _password),
                    
                };

            var request = new RestRequest(Method.POST);
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


            IRestResponse<Dictionary<string, object>> result = await client.ExecutePostTaskAsync<Dictionary<string, object>>(request);

            return GetId(result);
        }


         

        public async Task<(Guid id, string version)> AttachImageAsync(Guid nodeId, string name, byte[] image,
            FileOptions options)
        {
            string uri =
                $"{BaseAddress}/{Request}/nodes/{nodeId}/children";
            var client =
                new RestClient(uri)
                {
                    Authenticator = new HttpBasicAuthenticator(_username, _password)
                };

            var request = new RestRequest(Method.POST);
            request.AddParameter("name", name);
            request.AddParameter("overwrite", options.Overwrite);
            if (options.Title != null) request.AddParameter("cm:title", options.Title);
            if (options.Description != null) request.AddParameter("cm:description", options.Description);
            if (!string.IsNullOrWhiteSpace(options.Comment))
            {
                request.AddParameter("comment", options.Comment);
            }

            if (!string.IsNullOrWhiteSpace(options.RelativePath))
            {
                request.AddParameter("relativePath", options.RelativePath);
            }


            request.AddFile("filedata", image, name, "image/jpeg");


            IRestResponse<Dictionary<string, object>> result = await client.ExecutePostTaskAsync<Dictionary<string, object>>(request);

            return GetId(result);
        }


        /// <summary>
        /// Opens the file.
        /// </summary>
        /// <param name="fileId">The file identifier.</param>
        /// <param name="fileName">Name of the file.</param>
        public void OpenFile(Guid fileId, string fileName)
        {
            string uri = $"{BaseAddress}/{Request}/nodes/{fileId}/content";
            
            var client =
                new RestClient(uri)
                {
                    Authenticator = new HttpBasicAuthenticator(_username, _password)
                };

            var request = new RestRequest(fileName, Method.GET);

            string fileToWriteTo = Path.GetRandomFileName();

            string tempDirectory = Path.GetTempPath();

            string directory = Path.Combine(tempDirectory, fileToWriteTo);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (fileName.IndexOf(".pdf", StringComparison.Ordinal) == -1)
            {
                fileName = $"{fileName}.pdf";
            }

            string path = Path.Combine(directory, fileName);


            client.DownloadData(request).SaveAs(path);


            Process.Start(path, @"/r");
        }

        public string DownloadFile(Guid fileId, string version, string fileName)
        {
            string uri = $"{BaseAddress}/{Request}/nodes/{fileId}/versions/{version}/content";
            var client =
                new RestClient(uri)
                {
                    Authenticator = new HttpBasicAuthenticator(_username, _password)
                };

            var request = new RestRequest(fileName, Method.GET);


            string fileToWriteTo = Path.GetRandomFileName();



            string tempDirectory = Path.GetTempPath();

            string directory = Path.Combine(tempDirectory, fileToWriteTo);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if(!Path.HasExtension(fileName))
            {
                fileName = $"{fileName}.pdf";
            }
            //if (fileName.IndexOf(".pdf", StringComparison.Ordinal) == -1)
            //{
            //    fileName = $"{fileName}.pdf";
            //}

            string path = Path.Combine(directory, fileName);
            //application/vnd.openxmlformats-officedocument.wordprocessingml.document;charset=UTF-8

           
            client.DownloadData(request)
                .SaveAs(path);
            
            return path;
        }

        public void OpenFile(Guid fileId, string version, string fileName)
        {
            var path = DownloadFile(fileId, version, fileName);
            Process.Start(path, @"/r");
        }


        /// <summary>
        /// Links the file.
        /// </summary>
        /// <param name="sourceFileId">The source file identifier.</param>
        /// <param name="destinationFolderId">The destination folder identifier.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns><c>true</c> if successfully created link, <c>false</c> otherwise.</returns>
        public async Task<bool> LinkFile(Guid sourceFileId, Guid destinationFolderId, string fileName)
        {
            string uri = $"{BaseAddress}/{Request}/nodes/{destinationFolderId}/children";
            var client =
                new RestClient(uri)
                {
                    Authenticator = new HttpBasicAuthenticator(_username, _password)
                };

            var request = new RestRequest(Method.POST);
            request.AddParameter("name", fileName);
            request.AddParameter("nodeType", "app:filelink");
            request.AddParameter("cm:destination", sourceFileId);
            IRestResponse response = await client.ExecuteTaskAsync(request);
            return response.IsSuccessful;
        }


        public async Task<bool> CopyFile(Guid sourceFileId, Guid destinationFolderId, string fileName)
        {
            string uri = $"{BaseAddress}/{Request}/nodes/{sourceFileId}/copy";
            var client =
                new RestClient(uri)
                {
                    Authenticator = new HttpBasicAuthenticator(_username, _password)
                };

            var request = new RestRequest(Method.POST);
            request.AddParameter("name", fileName);
            request.AddParameter("targetParentId", destinationFolderId);
            IRestResponse response = await client.ExecuteTaskAsync(request);
            return response.IsSuccessful;
        }

        public async Task<bool> MoveFile(Guid sourceFileId, Guid destinationFolderId, string fileName)
        {

            string uri = $"{BaseAddress}/{Request}/nodes/{sourceFileId}/move";
            var client =
                new RestClient(uri)
                {
                    Authenticator = new HttpBasicAuthenticator(_username, _password)
                };

            var request = new RestRequest(Method.POST);
            request.AddParameter("name", fileName);
            request.AddParameter("targetParentId", destinationFolderId);
            IRestResponse response = await client.ExecuteTaskAsync(request);
            return response.IsSuccessful;
        }


        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="nodeId">The file or folder identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public async Task<bool> DeleteNode(Guid nodeId)
        {
            string uri = $"{BaseAddress}/{Request}/nodes/{nodeId}";
            var client =
                new RestClient(uri)
                {
                    Authenticator = new HttpBasicAuthenticator(_username, _password)
                };

            var request = new RestRequest(Method.DELETE);
            IRestResponse response = await client.ExecuteTaskAsync(request);
            return response.IsSuccessful;
        }


        /// <summary>
        /// Creates the folder.
        /// </summary>
        /// <param name="parentFolderId">The parent folder identifier.</param>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <returns>Task&lt;Guid&gt;.</returns>
        public async Task<(Guid id, string version)> CreateFolder(Guid parentFolderId, string folderName, string title,
            string description)
        {
            string uri = $"{BaseAddress}/{Request}/nodes/{parentFolderId}/children";
            var client =
                new RestClient(uri)
                {
                    Authenticator = new HttpBasicAuthenticator(_username, _password)
                };

            var request = new RestRequest(Method.POST);
            request.AddParameter("name", folderName);
            request.AddParameter("nodeType", "cm:folder");
            request.AddParameter("properties", "{" +
                                               $"  \"cm:title\":\"{title}\"," +
                                               $"  \"cm:description\":\"{description}\"," +
                                               "}");
            IRestResponse<Dictionary<string, object>> result = await client.ExecutePostTaskAsync<Dictionary<string, object>>(request);

            return GetId(result);
        }

        public byte[] GetImage(Guid signatureFileId)
        {
            string uri = $"{BaseAddress}/{Request}/nodes/{signatureFileId}/content";
            var client =
                new RestClient(uri)
                {
                    Authenticator = new HttpBasicAuthenticator(_username, _password)
                };

            var request = new RestRequest(Method.GET);

            byte[] result = client.DownloadData(request);
            return result;
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

        public void OpenWordFile(Guid fileId, string version, string fileName)
        {
            string uri = $"{BaseAddress}/{Request}/nodes/{fileId}/content";
            
            var client =
                new RestClient(uri)
                {
                    Authenticator = new HttpBasicAuthenticator(_username, _password)
                };

            var request = new RestRequest(fileName, Method.GET);

            string fileToWriteTo = Path.GetRandomFileName();

            string tempDirectory = Path.GetTempPath();

            string directory = Path.Combine(tempDirectory, fileToWriteTo);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (fileName.IndexOf(".docx", StringComparison.Ordinal) == -1)
            {
                fileName = $"{fileName}.docx";
            }

            string path = Path.Combine(directory, fileName);

            
            client.DownloadData(request).SaveAs(path);


            Process.Start(path, @"/r");
        }

        public void OpenWordFile(Guid fileId, string fileName)
        {
            string uri = $"{BaseAddress}/{Request}/nodes/{fileId}/content";
            
            var client =
                new RestClient(uri)
                {
                    Authenticator = new HttpBasicAuthenticator(_username, _password)
                };

            var request = new RestRequest(fileName, Method.GET);

            string fileToWriteTo = Path.GetRandomFileName();

            string tempDirectory = Path.GetTempPath();

            string directory = Path.Combine(tempDirectory, fileToWriteTo);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (fileName.IndexOf(".docx", StringComparison.Ordinal) == -1)
            {
                fileName = $"{fileName}.docx";
            }

            string path = Path.Combine(directory, fileName);


            client.DownloadData(request).SaveAs(path);


            Process.Start(path, @"/r");
        }
    }


   
}