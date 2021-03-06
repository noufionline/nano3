using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PostSharp.Patterns.Caching;
using PostSharp.Patterns.Caching.Backends.Redis;
using StackExchange.Redis;

namespace Jasmine.AbsCore.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SetupPostSharpCaching();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseStartup<Startup>();
                });


        private static void SetupPostSharpCaching()
        {
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect("localhost");
            connection.ErrorMessage += (sender, eventArgs) => Console.Error.WriteLine(eventArgs.Message);
            connection.ConnectionFailed += (sender, eventArgs) => Console.Error.WriteLine(eventArgs.Exception);

            var configuration = new RedisCachingBackendConfiguration { IsLocallyCached = true, SupportsDependencies = true };


            var backend = RedisCachingBackend.Create(connection, configuration);
            // With Redis, we need at least one instance of the collection engine.
            using (RedisCacheDependencyGarbageCollector.Create(connection, configuration))
            {
                CachingServices.DefaultBackend = backend;
            }
        }
    }
}
