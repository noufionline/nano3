using System;
using System.IO;
using System.Reflection;
using Autofac;
using AutoMapper;
using Jasmine.Abs.Api.PolicyServer;
using Jasmine.Abs.Entities.Models.Azman;
using Jasmine.Abs.Entities.Models.Core;
using Jasmine.Abs.Entities.Models.Zeon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Z.EntityFramework.Extensions;

namespace Jasmine.Abs.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddDbContext<AbsContext>(builder =>
                builder.UseSqlServer(Configuration.GetConnectionString("CICONABS")), ServiceLifetime.Transient);

            services.AddDbContext<ZeonContext>(builder =>
                builder.UseSqlServer(Configuration.GetConnectionString("CICONIDP")), ServiceLifetime.Transient);

            services.AddDbContext<NetSqlAzmanContext>(builder =>
                builder.UseSqlServer(Configuration.GetConnectionString("NetSqlAzman")), ServiceLifetime.Transient);

            services.Configure<ConnectionStringConfiguration>(Configuration.GetSection("ConnectionStrings"));


            services.AddControllers(options =>
            {
                options.Filters.Add(
                       new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
                options.Filters.Add(
                    new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
                options.Filters.Add(
                    new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
                options.Filters.Add(
                    new ProducesDefaultResponseTypeAttribute());
                options.Filters.Add(
                    new ProducesResponseTypeAttribute(StatusCodes.Status401Unauthorized));

                options.ReturnHttpNotAcceptable = true;

                if (_env.IsDevelopment())
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAssertion(_ => true)
                        .Build();

                    options.Filters.Add(new AuthorizeFilter(policy));

                    services.AddTestAuthorization()
                        .AddAuthorizationPermissionPolicies();


                }
                else
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                    options.Filters.Add(new AuthorizeFilter(policy));

                    services.AddAbsAuthorization()
                       .AddAuthorizationPermissionPolicies();
                }


                services.Configure<ApiBehaviorOptions>(options =>
               {
                   options.InvalidModelStateResponseFactory = actionContext =>
                   {
                       var actionExecutingContext =
                           actionContext as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;

                       // if there are modelstate errors & all keys were correctly
                       // found/parsed we're dealing with validation errors
                       if (actionContext.ModelState.ErrorCount > 0
                          && actionExecutingContext?.ActionArguments.Count == actionContext.ActionDescriptor.Parameters.Count)
                       {
                           return new UnprocessableEntityObjectResult(actionContext.ModelState);
                       }

                       // if one of the keys wasn't correctly found / couldn't be parsed
                       // we're dealing with null/unparsable input
                       return new BadRequestObjectResult(actionContext.ModelState);
                   };
               });

                services.AddAuthentication("Bearer")
               .AddIdentityServerAuthentication(options =>
               {
                   if (_env.IsProduction())
                   {
                       options.Authority = "https://abs.cicononline.com/zeon";
                       options.RequireHttpsMetadata = true;
                   }
                   else
                   {
                       options.Authority = "https://localhost:5001";
                       options.RequireHttpsMetadata = true;
                   }

                   options.ApiName = "abscoreapi";
                   options.SaveToken = true;
               });


                services.AddAutoMapper(typeof(Startup).GetTypeInfo().Assembly);

#if DEBUG
                services.AddSwaggerGen(setupAction =>
                {
                    setupAction.SwaggerDoc("LibraryOpenAPISpecification",
                        new OpenApiInfo()
                        {
                            Title = "Library API",
                            Version = "1"
                        });

                    //  setupAction.AddFluentValidationRules();

                    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                    setupAction.IncludeXmlComments(xmlCommentsFullPath);

                });

#endif


            });
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
        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
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
