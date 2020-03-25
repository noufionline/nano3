using GraphQL.Authorization;
using GraphQL.Types;
using Jasmine.Abs.Api.Dto.Abs;
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

    public class AbsCustomerMutation : ObjectGraphType
    {
        public AbsCustomerMutation()
        {
            FieldAsync<CustomerType>("createCustomer", arguments: new QueryArguments(
                 new QueryArgument<NonNullGraphType<CustomerInputType>> { Name = "customer" }),
             resolve: async context =>
             {
                 this.AuthorizeWith("CanImportFromSunSystem");
                 return await context.TryAsyncResolve(
                async c => await Task.FromResult(new CustomerDto { CustomerId = 1, CustomerName = "Test" }));
             });
        }
    }
       
}
