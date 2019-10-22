using Jasmine.Abs.Api.Dto.Abs;
using Jasmine.Abs.Api.Repositories.Abs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jasmine.Abs.Api.Controllers.Lookup.Abs
{
    [Route("api/abs/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _repository;

        public CustomerController(ICustomerRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        ///  Gets Customers from ABS   
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetCustomersFromAbs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<CustomerDto>> Get()
        {
            return Ok(await _repository.GetCustomersAsync()); 
        }
    }


  
}
