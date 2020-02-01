using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using GrpcService.AutoMapper;
using IdentityServer4.AccessTokenValidation;
using Jasmine.Abs.Api.PolicyServer;
using Jasmine.Abs.Entities.Models.Azman;
using Jasmine.Abs.Entities.Models.Zeon;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GrpcService
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940


        private readonly IWebHostEnvironment _environment;

        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddHttpContextAccessor();

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("protectedScope", policy =>
            //    {
            //        policy.RequireClaim("scope", "grpc_protected_scope");
            //    });
            //});

            services.AddAbsAuthorization()
              .AddAuthorizationPermissionPolicies();

            //  services.AddAuthorizationPolicyEvaluator();

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                 .AddIdentityServerAuthentication(options =>
                {
                    //if (_environment.IsProduction())
                    //{
                    options.Authority = "https://abs.cicononline.com/zeon";
                    options.RequireHttpsMetadata = true;
                    //}
                    //else
                    //{
                    //    options.Authority = "https://localhost:5001";
                    //    options.RequireHttpsMetadata = true;
                    //}

                    options.ApiName = "abscoreapi";
                    options.SaveToken = true;
                });


            //if (_environment.IsDevelopment())
            //{
            //services.AddTestAuthorization()
            //        .AddAuthorizationPermissionPolicies();
                // services.For<IAuthorizationService>().Use<TestAuthorizationService>();

            //}
            //else
            //{


            //}

            services.AddGrpc(options =>
            {
                options.EnableDetailedErrors = true;
            });

            services.AddAutoMapper(config =>
            {
                config.CreateMap<DateTime, Timestamp>()
                .ConvertUsing(d => d == DateTime.MinValue ? Timestamp.FromDateTimeOffset(DateTimeOffset.MinValue) : Timestamp.FromDateTimeOffset(d));

                config.CreateMap<Timestamp, DateTime>()
                .ConvertUsing(s => s.ToDateTimeOffset().LocalDateTime);

                config.CreateMap<string, string>().ConvertUsing<NullStringConverter>();

                config.CreateMap<decimal, DecimalValue>().ConvertUsing(s => DecimalValue.FromDecimal(s));

            }, typeof(Startup).GetTypeInfo().Assembly);

            services.AddDbContext<ZeonContext>(builder =>
                builder.UseSqlServer(Configuration.GetConnectionString("CICONIDP")), ServiceLifetime.Transient);
            
            services.AddDbContext<NetSqlAzmanContext>(builder =>
                builder.UseSqlServer(Configuration.GetConnectionString("NetSqlAzman")), ServiceLifetime.Transient);

        }



        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutoFacModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GreeterService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
