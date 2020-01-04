using GraphQL;
using GraphQL.Types;
using GraphQL.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jasmine.Abs.Api.GraphQL
{
    public class AbsCustomerSchema:Schema
    {
        public AbsCustomerSchema(IServiceProvider provider):base(provider)
        {
            Query=provider.GetRequiredService<AbsCustomerQuery>();
        }
    }

    
}
