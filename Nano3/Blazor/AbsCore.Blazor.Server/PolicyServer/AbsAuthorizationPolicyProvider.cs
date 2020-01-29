using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace AbsCore.Blazor.PolicyServer
{

    public class AbsAuthorizationPolicyProvider:DefaultAuthorizationPolicyProvider
    {
        public AbsAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
        {
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policy =await base.GetPolicyAsync(policyName);
            if (policy == null)
            {
                policy=new AuthorizationPolicyBuilder().AddRequirements(new AbsPermissionRequirement(policyName)).Build();
            }

            return policy;
        }
    }
}