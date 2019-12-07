using System.Threading.Tasks;

namespace Jasmine.Core.Contracts
{
    public interface IApiTokenProvider
    {
        bool HasToken { get; }
        string AccessToken { get; }
        string RefreshToken { get; }
        string ExpiresAt { get; }
        void ReSet(string accessToken, string refreshToken, string expiresAt);
        Task<string> GetTokenAsync();
        string GetApiEndPoint();
        string GetAuthority();
    }
}