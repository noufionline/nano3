using System;
using System.Threading.Tasks;

namespace PrismSampleApp.Services
{
    public interface IAlfrescoClient
    {
        Task<(System.Guid id, string version)> AttachFileAsync(System.Guid nodeId, string name, string path, FileOptions options);
        Task OpenFileAsync(Guid fileId);
    }
}