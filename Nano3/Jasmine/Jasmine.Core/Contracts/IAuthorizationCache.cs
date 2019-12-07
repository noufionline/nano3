using System.Threading.Tasks;

namespace Jasmine.Core.Contracts
{
    public interface IAuthorizationCache
    {
        bool CheckAccess(string operation);
        Task InvalidateCacheAsync();
        bool CheckLookupItemAccess(string route);
    }
}