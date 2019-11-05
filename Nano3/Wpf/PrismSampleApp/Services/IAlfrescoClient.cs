using System.Threading.Tasks;

namespace PrismSampleApp.Services
{
    public interface IAlfrescoClient
    {
        Task OpenFileAsync(string fileId,string userName,string password);
    }
}