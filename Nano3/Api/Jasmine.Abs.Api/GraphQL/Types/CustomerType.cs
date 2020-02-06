using GraphQL.Types;
using Jasmine.Abs.Entities.Models.Abs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jasmine.Abs.Api.Dto.Abs;


namespace Jasmine.Abs.Api.GraphQL.Types
{
    public class CustomerType:ObjectGraphType<CustomerDto>
    {
        public CustomerType()
        {
            Field(t=> t.CustomerId);
            Field(t=> t.CustomerName);
            Field(t=> t.SunAccountCode);
            Field(t=> t.VatRegNo);
            Field(t=> t.NameOnTradeLicense);
        }
    }


   
}
