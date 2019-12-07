using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Dapper;
using Dapper.FluentMap;
using GrpcService.TypeMappers;
using Microsoft.AspNetCore.Hosting;
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
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseWindowsService()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
