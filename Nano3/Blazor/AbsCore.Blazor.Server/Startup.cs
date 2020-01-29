using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AbsCore.Blazor.Server.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Polly;
using DevExpress.Logify.Web;
using Syncfusion.EJ2.Blazor;
using AbsCore.Blazor.PolicyServer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;

namespace AbsCore.Blazor.Server
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddRazorPages();
            services.AddServerSideBlazor().AddCircuitOptions(cfg => cfg.DetailedErrors = true); ;
            services.AddSyncfusionBlazor();




            services.AddAbsAuthorization()
               .AddAuthorizationPermissionPolicies();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {

                options.Authority = _env.IsDevelopment() ? "https://localhost:44311" : "https://abs.cicononline.com/idp";


                options.ClientId = "abscoreblazor";
                options.ClientSecret = "5651841c-9615-4d1e-bf79-81a382faac81";

                options.ResponseType = "code id_token";

                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.Scope.Add("abscoreapi");
                options.Scope.Add("offline_access");


                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;

                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role",
                };

            });

            // adds user and client access token management
            services.AddAccessTokenManagement(options =>
                {
                    // client config is inferred from OpenID Connect settings
                    // if you want to specify scopes explicitly, do it here, otherwise the scope parameter will not be sent
                    options.Client.Scope = "abscoreapi";
                })
                .ConfigureBackchannelHttpClient()
                    .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(new[]
                    {
                        TimeSpan.FromSeconds(1),
                        TimeSpan.FromSeconds(2),
                        TimeSpan.FromSeconds(3)
                    }));




            services.AddHttpClient<ILcDocumentService, LcDocumentService>(client =>
               {
                   client.BaseAddress = _env.IsDevelopment() ?
                   new Uri("https://localhost:5051/api/") : new Uri("https://abs.cicononline.com/abscoreapi/api/");
               }).AddUserAccessTokenHandler();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTkyNzY0QDMxMzcyZTM0MmUzMFlYMmV6czlKUkc1V2JnM3NLMGFtbWRjeE5XNm9OcGV0ZUdkVHViT3BmZ289");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseLogifyAlert(Configuration.GetSection("LogifyAlert"));

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
