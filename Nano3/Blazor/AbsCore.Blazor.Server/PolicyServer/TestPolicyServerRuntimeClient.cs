using PolicyServer.Runtime.Client;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AbsCore.Blazor.PolicyServer
{
    public class TestPolicyServerRuntimeClient:IPolicyServerRuntimeClient
    {

        #region Implementation of IPolicyServerRuntimeClient

        public Task<PolicyResult> EvaluateAsync(ClaimsPrincipal user)
        {   
            return Task.FromResult(new PolicyResult());
        }

        public Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permission)
        {
            if(permission == "CanManageLCDocuments")
            {
               return Task.FromResult(user.Identity.IsAuthenticated);
            }
            return Task.FromResult(true);
        }

        public Task<bool> IsInRoleAsync(ClaimsPrincipal user, string role)
        {
            return Task.FromResult(true);
        }

        #endregion
    }
}
