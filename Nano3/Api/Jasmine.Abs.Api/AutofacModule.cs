using Autofac;
using Autofac.Core;
using Jasmine.Abs.Api.Repositories.Contracts;
using Jasmine.Abs.Entities.Models.Abs;
using Jasmine.Abs.Entities.Models.Azman;
using Jasmine.Abs.Entities.Models.Zeon;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;

namespace Jasmine.Abs.Api
{
    internal class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var dataAccess = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(dataAccess)
                   .Where(t => t.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces();

  
            builder.RegisterGeneric(typeof(LookupItemRepository<>)).As(typeof(ILookupItemRepository<>));


            builder.Register(context =>
            {
                var cs = context.Resolve<IConfiguration>()
                    .GetConnectionString("CICONIDP");

                var optionsBuilder = new DbContextOptionsBuilder<ZeonContext>()
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    .UseSqlServer(cs);

                return new ZeonContext(optionsBuilder.Options);

            }).InstancePerLifetimeScope();

            builder.Register(context =>
            {

                var httpContextAccessor = context.Resolve<IHttpContextAccessor>();
                if (httpContextAccessor.HttpContext.Request.Headers.TryGetValue("db", out var db))
                {
                    var cs = context.Resolve<IConfiguration>()
                   .GetConnectionString("CICONABS");

                    var connectionString = new SqlConnectionStringBuilder(cs) { InitialCatalog = db,DataSource="192.168.30.26" }.ToString();

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