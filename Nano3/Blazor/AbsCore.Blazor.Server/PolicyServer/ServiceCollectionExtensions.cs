using Microsoft.Extensions.DependencyInjection;
using PolicyServer.Runtime.Client;

namespace AbsCore.Blazor.PolicyServer
{
    public static class ServiceCollectionExtensions
    {
        public static AbsPolicyServerBuilder AddAbsAuthorization(this IServiceCollection services)
        {
            services.AddTransient<IPolicyServerRuntimeClient, TestPolicyServerRuntimeClient>();
            return new AbsPolicyServerBuilder(services);
        }

        public static AbsPolicyServerBuilder AddTestAuthorization(this IServiceCollection services)
        {
            services.AddTransient<IPolicyServerRuntimeClient, TestPolicyServerRuntimeClient>();
            return new AbsPolicyServerBuilder(services);
        }
    }
}