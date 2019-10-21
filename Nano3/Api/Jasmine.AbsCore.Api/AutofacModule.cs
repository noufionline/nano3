using Autofac;
using Autofac.Core;
using Jasmine.AbsCore.Entities.Models.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Jasmine.AbsCore.Api
{
    internal class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var dataAccess = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(dataAccess)
                   .Where(t => t.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces();


          //builder.Register(context=> 
          //{   
          //     var cs = context.Resolve<IConfiguration>()
          //          .GetConnectionString("CICONABS");

          //      var optionsBuilder = new DbContextOptionsBuilder<AbsContext>()
          //          .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
          //          .UseSqlServer(cs);

          //      var db = new AbsContext(optionsBuilder.Options);

          //    //  db.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());

          //      return db;
          //});
            
        }
    }
}