using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Jasmine.Blazor.Server.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using DevExpress.Logify.Web;
using DevExpress.AspNetCore;
using Polly;
using System.Net.Http.Headers;

namespace Jasmine.Blazor.Server
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
            services.AddControllers();
            services.AddRazorPages();
            services.AddServerSideBlazor(options => options.DetailedErrors = true);
            services.AddDevExpressControls();


            services.AddHttpContextAccessor();

            services.AddSingleton<WeatherForecastService>();


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
                  
                  options.SaveTokens = true;
                  options.GetClaimsFromUserInfoEndpoint = true;
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
                 new Uri("https://localhost:5051/api/")  : new Uri("https://abs.cicononline.com/abscoreapi/api/");
             }).AddUserAccessTokenHandler();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
