using System;
using System.Data.SqlClient;
using Autofac;
using GrpcService.Contracts;
using Jasmine.Abs.Entities.Models.Abs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GrpcService
{
    public class AutoFacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AbsConnectionStringProvider>().As<IAbsConnectionStringProvider>();
            builder.Register(context =>
           {

               var httpContextAccessor = context.Resolve<IHttpContextAccessor>();
               if (httpContextAccessor.HttpContext.Request.Headers.TryGetValue("db", out var db))
               {
                  // var cs = context.Resolve<IConfiguration>()
                  //.GetConnectionString("CICONABS");

                   var cs="Data Source=192.168.30.26; Initial Catalog=ABS_CBF2; User Id=sa;pwd=fkt";

                   var connectionString = new SqlConnectionStringBuilder(cs) { InitialCatalog = db }.ToString();

                   var optionsBuilder = new DbContextOptionsBuilder<AbsClassicContext>()
                   .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                   .UseSqlServer(connectionString);

                   return new AbsClassicContext(optionsBuilder.Options);
               }
               else
               {
                   throw new ArgumentOutOfRangeException("abs db name not specified in request header");
               }

           }).InstancePerLifetimeScope();


            
            
        }
    }
}
