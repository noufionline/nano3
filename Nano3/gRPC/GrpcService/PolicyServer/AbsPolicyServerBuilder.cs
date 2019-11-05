using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Jasmine.Abs.Api.PolicyServer
{
    public class AbsPolicyServerBuilder
    {
        public IServiceCollection Services { get; }

        public AbsPolicyServerBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public AbsPolicyServerBuilder AddAuthorizationPermissionPolicies()
        {
            Services.AddAuthorization();
            Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            Services.AddTransient<IAuthorizationPolicyProvider, AbsAuthorizationPolicyProvider>();
            Services.AddTransient<IAuthorizationHandler, AbsPermissionHandler>();
            return this;
        }
    }
}