using Autofac;
using Autofac.Core;
using Jasmine.Abs.Entities.Models.Azman;
using Jasmine.Abs.Entities.Models.Zeon;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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



            builder.Register(context =>
            {
                var cs = context.Resolve<IConfiguration>()
                    .GetConnectionString("CICONIDP");

                var optionsBuilder = new DbContextOptionsBuilder<ZeonContext>()
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    .UseSqlServer(cs);

                return new ZeonContext(optionsBuilder.Options);

            }).InstancePerRequest();


            builder.Register(context =>
            {
                var cs = context.Resolve<IConfiguration>()
                    .GetConnectionString("NetSqlAzman");

                var optionsBuilder = new DbContextOptionsBuilder<NetSqlAzmanContext>()
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    .UseSqlServer(cs);
                return new NetSqlAzmanContext(optionsBuilder.Options);

            }).InstancePerRequest();


            //builder.Register(context =>
            //{
            //    var cs = context.Resolve<IConfiguration>().GetConnectionString("MSSQL2014").WithDb("ABS_SMS");

            //    var optionsBuilder = new DbContextOptionsBuilder<SmsContext>()
            //        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            //        .UseSqlServer(cs);

            //    return new SmsContext(optionsBuilder.Options);
            //}).Transient();

           // For<CiconContext>().Use(context =>
           // {
           //    var cs = context.GetInstance<IConfiguration>()
           //        .GetConnectionString("MSSQL2014")
           //        .WithDb("CICON");

           //    var optionsBuilder = new DbContextOptionsBuilder<CiconContext>()
           //        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
           //        .UseSqlServer(cs);

           //    return new CiconContext(optionsBuilder.Options);
           //}).Transient();


            //For<LmsContext>().Use(context =>
            //{
            //    var cs = context.GetInstance<IConfiguration>()
            //        .GetConnectionString("CICONABS")
            //        .WithDb("Logistics");

            //    var optionsBuilder = new DbContextOptionsBuilder<LmsContext>()
            //        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            //        .UseSqlServer(cs);

            //    return new LmsContext(optionsBuilder.Options);
            //}).Transient();
        }
    }
}