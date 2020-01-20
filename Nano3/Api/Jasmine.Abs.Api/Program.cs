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
using Serilog;
using StackExchange.Redis;

namespace Jasmine.Abs.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {

#if DEBUG
            HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
#endif
            ConfigurePostSharpCache();

            CreateHostBuilder(args).Build().Run();
        }

        private static void ConfigurePostSharpCache()
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

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseUrls("http://localhost:5050", "https://localhost:5051")
                .ConfigureLogging(logging =>
                {
                    logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Trace);
                    logging.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Debug);
                })
                .UseSerilog()
                .UseStartup<Startup>();
                });
    }
}
