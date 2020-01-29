using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WindowsServiceHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var certifcatePath=Path.Combine(AppContext.BaseDirectory, "grpc.cicononline.com.pfx");
                    var cert = new X509Certificate2(certifcatePath, "MtpsF42");
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel(kestrelServerOptions =>
                    {
                        kestrelServerOptions.ConfigureHttpsDefaults(opt =>
                    {
                        opt.ClientCertificateMode = ClientCertificateMode.AllowCertificate;
                        opt.CheckCertificateRevocation=true;
                        opt.ServerCertificate=cert;
                        //// Verify that client certificate was issued by same CA as server certificate
                        //opt.ClientCertificateValidation = (certificate, chain, errors) =>
                        //    certificate.Issuer == cert.Issuer;
                    });
                    });
                    webBuilder.UseUrls("https://*:8443");
                });
    }
}
