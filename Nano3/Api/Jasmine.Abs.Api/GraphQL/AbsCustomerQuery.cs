using GraphQL.Types;
using Jasmine.Abs.Api.GraphQL.Types;
using Jasmine.Abs.Api.Repositories.Abs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jasmine.Abs.Api.GraphQL
{
    public class AbsCustomerQuery:ObjectGraphType
    {
        public AbsCustomerQuery(ICustomerRepository repository)
        {
            Field<ListGraphType<CustomerType>>("customers",resolve: context => repository.GetCustomersAsync());
        }
    }
}
