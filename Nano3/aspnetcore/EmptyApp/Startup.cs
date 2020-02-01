using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EmptyApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IConfiguration configuration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    var sb = new StringBuilder();
                    var nl = System.Environment.NewLine;
                    var rule = string.Concat(nl, new string('-', 40), nl);

                    sb.AppendLine(configuration.GetConnectionString("CICONABS"));
                    sb.AppendLine(configuration.GetConnectionString("CICONIDP"));
                    sb.AppendLine(configuration.GetConnectionString("ABS"));

                    sb.Append($"Environment Variables{rule}");
                    var vars = System.Environment.GetEnvironmentVariables();
                    foreach (var key in vars.Keys.Cast<string>().OrderBy(key => key,
                        StringComparer.OrdinalIgnoreCase))
                    {
                        var value = vars[key];
                        sb.Append($"{key}: {value}{nl}");
                    }

                    context.Response.ContentType = "text/plain";
                    //Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)
                    await context.Response.WriteAsync(sb.ToString());
                });
            });
        }
    }
}
