using Jasmine.Abs.Api.Dto.Abs;
using Jasmine.Abs.Api.Repositories.Contracts;
using Jasmine.Abs.Entities.Models.Abs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jasmine.Abs.Api.Repositories.Abs
{
    public class CustomerRepository:ICustomerRepository
    {
        private readonly AbsClassicContext _context;

        public CustomerRepository(AbsClassicContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CustomerDto>> GetCustomersAsync()
        {
            return await _context.Customers.Select(x=> new CustomerDto
            { 
                CustomerId=x.CustomerId,
                CustomerName=x.CustomerName,
                SunAccountCode=x.SunAccountCode ?? string.Empty

            }).ToListAsync().ConfigureAwait(false);
        }
    }

    public interface ICustomerRepository
    {
        Task<IEnumerable<CustomerDto>> GetCustomersAsync();
    }
}
