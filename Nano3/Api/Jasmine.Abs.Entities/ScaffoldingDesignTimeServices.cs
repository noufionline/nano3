﻿using EntityFrameworkCore.Scaffolding.Handlebars;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Jasmine.Abs.Entities
{
    public partial class ScaffoldingDesignTimeServices : IDesignTimeServices
    {
        public void ConfigureDesignTimeServices(IServiceCollection services)
        {
           // ReverseEngineerOptions options = ReverseEngineerOptions.DbContextAndEntities;

            // Add custom template data
    
            services.AddHandlebarsScaffolding(options=> 
            {
                options.ReverseEngineerOptions =ReverseEngineerOptions.DbContextAndEntities;
                options.TemplateData = new Dictionary<string, object>
                {
                    { "base-class", "TrackableEntityBase" }
                };
            });

           // services.AddTransient<ICSharpDbContextGenerator, JasmineHbsCSharpDbContextGenerator>();
            // Register Handlebars helper
            //var myHelper = (helperName: "my-helper", helperFunction: (Action<TextWriter, object, object[]>)MyHbsHelper);

            // Add Handlebars scaffolding templates
            //services.AddHandlebarsScaffolding(options);

            // Register Handlebars helper
            //services.AddHandlebarsHelpers(myHelper);


            services.AddHandlebarsTransformers(propertyTransformer: e => PropertyTransformer(e));

         //   Inflector.Inflector.SetDefaultCultureFunc = () => Thread.CurrentThread.CurrentUICulture;
         //   services.AddSingleton<IPluralizer, MyPluralizer>();
        }
        EntityPropertyInfo PropertyTransformer(EntityPropertyInfo propertyInfo)
        {
            switch (propertyInfo.PropertyName)
            {
                case "PaymentStatusId":
                    return new EntityPropertyInfo(typeof(PaymentStatusTypes).Name, "PaymentStatusId");
                case "DocumentType":
                    return new EntityPropertyInfo(typeof(AccountReceivableTypes).Name, "DocumentType");
                case "CommercialInvoiceStatus":
                    return new EntityPropertyInfo(typeof(CommercialInvoiceStatusTypes).Name, "CommercialInvoiceStatus");
                default:
                    return new EntityPropertyInfo(propertyInfo.PropertyType, propertyInfo.PropertyName);
            }
        }

        void MyHbsHelper(TextWriter writer, object context, object[] parameters)
        {
            writer.Write("// My Handlebars Helper");
        }

    }
}
