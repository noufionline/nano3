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
using Jasmine.Blazor.Server.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Jasmine.Blazor.Server
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration,IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();

            services.AddAuthentication(options =>
          {
              options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
              options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
          })
              .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
              {
                  
                  if(_env.IsDevelopment())
                  {
                      options.Authority = "https://localhost:44311";
                  }
                  else
                  {
                      options.Authority = "https://abs.cicononline.com/idp";
                  }
                  

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

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
