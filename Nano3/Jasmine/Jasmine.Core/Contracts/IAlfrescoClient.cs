using Jasmine.Core.Alfresco;
using System;
using System.Threading.Tasks;

namespace Jasmine.Core.Contracts
{
    public interface IAlfrescoClient
    {
        string DownloadFile(Guid fileId, string version, string fileName);

        /// <summary>
        /// Gets the base address.
        /// </summary>
        /// <value>The base address.</value>
        string BaseAddress { get; }

        /// <summary>
        /// attach file as an asynchronous operation.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="path">The path.</param>
        /// <param name="options"></param>
        /// <returns>Task&lt;Guid&gt;.</returns>
        Task<(Guid id, string version)> AttachFileAsync(Guid nodeId, string name, string path, FileOptions options);
        Task<(Guid id, string version)> AttachImageAsync(Guid nodeId, string name, byte[] image, FileOptions options);

        /// <summary>
        /// Opens the file.
        /// </summary>
        /// <param name="fileId">The file identifier.</param>
        /// <param name="fileName">Name of the file.</param>
        void OpenFile(Guid fileId, string fileName);
        void OpenFile(Guid fileId, string version, string fileName);

        void OpenWordFile(Guid fileId,string version,string fileName);
        void OpenWordFile(Guid fileId, string fileName);

        /// <summary>
        /// Links the file.
        /// </summary>
        /// <param name="sourceFileId">The source file identifier.</param>
        /// <param name="destinationFolderId">The destination folder identifier.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns><c>true</c> if successfully created link, <c>false</c> otherwise.</returns>
        Task<bool> LinkFile(Guid sourceFileId, Guid destinationFolderId, string fileName);

        Task<bool> CopyFile(Guid sourceFileId, Guid destinationFolderId, string fileName);
        Task<bool> MoveFile(Guid sourceFileId, Guid destinationFolderId, string fileName);

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="nodeId">The file or folder identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        Task<bool> DeleteNode(Guid nodeId);

        /// <summary>
        /// Creates the folder.
        /// </summary>
        /// <param name="parentFolderId">The parent folder identifier.</param>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <returns>Task&lt;Guid&gt;.</returns>
        Task<(Guid id, string version)> CreateFolder(Guid parentFolderId, string folderName, string title,
            string description);


        //  Task CreateUser(AlfrescoUser user);

        byte[] GetImage(Guid signatureFileId);
    }

    public class AlfrescoUser
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

    }
}