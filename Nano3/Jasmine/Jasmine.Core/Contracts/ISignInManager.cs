using System.Security.Claims;
using System.Threading.Tasks;
using Jasmine.Core.Security;

namespace Jasmine.Core.Contracts
{
    public interface ISignInManager
    {
        Task<ClaimsPrincipal> SignInAsync(string userName, string password, int divisionId);
        Task<ClaimsPrincipal> SignInAsync(UserCredential credential);
        Task ChangeDivisionAsync(int divisionId);
        Task<bool> ValidateLoginAsync(string email, string password);
        Task<bool> SavePasswordAsync(string email, string password);
     
    }
}