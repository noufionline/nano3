using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Dapper;
using Dapper.FluentMap;
using GrpcService.TypeMappers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Hosting;

namespace GrpcService
{
    public class Program
    {
        public static void Main(string[] args)
        {


            FluentMapper.Initialize(cfg =>
           {
               cfg.AddMap(new SteelDeliveryNoteDetailReportDataMap());
               cfg.AddMap(new SalesAndServicesThreadingDataMap());
               cfg.AddMap(new SalesAndServicesOthersDataMap());
               cfg.AddMap(new MaterialTransferSteelDataMap());
               cfg.AddMap(new MaterialTransferOthersDataMap());
               cfg.AddMap(new OtherSteelDeliveryReportDataMap());
               cfg.AddMap(new ForProductionDataReportDataMap());

           });

            SqlMapper.AddTypeHandler(new TimestampMapper());


            CreateHostBuilder(args).Build().Run();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseContentRoot(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName))
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseWindowsService() // Enable running as a Windows service
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var certifcatePath = Path.Combine(AppContext.BaseDirectory, "grpc.cicononline.com.pfx");
                    var cert = new X509Certificate2(certifcatePath, "MtpsF42");
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel(kestrelServerOptions =>
                    {
                        kestrelServerOptions.ConfigureHttpsDefaults(opt =>
                        {
                            opt.ClientCertificateMode = ClientCertificateMode.AllowCertificate;
                            opt.CheckCertificateRevocation = true;
                            opt.ServerCertificate = cert;
                        // Verify that client certificate was issued by same CA as server certificate
                        opt.ClientCertificateValidation = (certificate, chain, errors) =>
                                certificate.Issuer == cert.Issuer;
                        });
                    });
                    webBuilder.UseUrls("https://*:8443");
                    //webBuilder.UseIISIntegration();
                });
    }
}
