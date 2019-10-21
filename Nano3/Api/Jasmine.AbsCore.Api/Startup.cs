using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Jasmine.AbsCore.Entities.Models.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Z.EntityFramework.Extensions;

namespace Jasmine.AbsCore.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;


            Serilog.Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Debug()
           .Enrich.FromLogContext()
           .WriteTo.Seq("http://localhost:5341")
           //.WriteTo.MSSqlServer(Configuration["Serilog:ConnectionString"], Configuration["Serilog:TableName"],autoCreateSqlTable: true)
           .CreateLogger();


            var licenseName = Configuration.GetValue<string>("Z.LicenseName");
            var licenseKey = Configuration.GetValue<string>("Z.LicenseKey");
            LicenseManager.AddLicense(licenseName, licenseKey);

            EntityFrameworkManager.ContextFactory = context =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<AbsContext>();
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("CICONABS"));
                return new AbsContext(optionsBuilder.Options);
            };

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<AbsContext>(x =>
                 x.UseSqlServer(Configuration.GetConnectionString("CICONABS")),ServiceLifetime.Transient);

        }



        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
