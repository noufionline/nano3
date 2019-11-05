using Microsoft.Extensions.DependencyInjection;
using PolicyServer.Runtime.Client;

namespace Jasmine.Abs.Api.PolicyServer
{
    public static class ServiceCollectionExtensions
    {
        public static AbsPolicyServerBuilder AddAbsAuthorization(this IServiceCollection services)
        {
            services.AddTransient<IPolicyServerRuntimeClient, AbsPolicyServerRuntimeClient>();
            return new AbsPolicyServerBuilder(services);
        }

        public static AbsPolicyServerBuilder AddTestAuthorization(this IServiceCollection services)
        {
            services.AddTransient<IPolicyServerRuntimeClient, TestPolicyServerRuntimeClient>();
            return new AbsPolicyServerBuilder(services);
        }
    }
}