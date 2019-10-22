using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jasmine.Abs.Api.Repositories;
using Jasmine.Abs.Api.Repositories.Contracts;
using Jasmine.Abs.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jasmine.Abs.Api.Controllers.Lookup.Login
{
    [Produces("application/json")]
    [Route("api/login/lookup/divisions")]
    public class DivisionController : Controller
    {
        private readonly ILoginRepository _repository;

        public DivisionController(ILoginRepository repository) => _repository = repository;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetDivisionsAsync()
        {
            var items = await _repository.GetDivisionsAsync().ConfigureAwait(false);
            if (items == null)
            {
                return NotFound();
            }



            //for (int i = 1000; i < 2000; i++)
            //{
            //   items.Add(new AbsApplicationInfo(){ApplicationType = $"Test {i}",Id = i,Name = $"Name {i}"});
            //}

            return Ok(items);
        }
    }
}