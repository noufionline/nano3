using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PolicyServer.Runtime.Client;

namespace Jasmine.Abs.Api.PolicyServer
{
    public class AbsPermissionHandler:AuthorizationHandler<AbsPermissionRequirement>
    {
        private readonly IPolicyServerRuntimeClient _client;

        public AbsPermissionHandler(IPolicyServerRuntimeClient client)
        {
            _client = client;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AbsPermissionRequirement requirement)
        {
            if (await _client.HasPermissionAsync(context.User, requirement.Name))
            {
                context.Succeed(requirement);
            }
        }
    }
}