using System.Collections.Generic;
using System.Threading.Tasks;
using Jasmine.Abs.Api.Repositories;
using Jasmine.Abs.Api.Repositories.Contracts;
using Jasmine.Abs.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jasmine.Abs.Api.Controllers.Lookup.Login
{
    [Produces("application/json")]
    [Route("api/login/lookup/users")]
    public class UserController : Controller
    {
        private readonly ILoginRepository _repository;

        public UserController(ILoginRepository repository) => _repository = repository;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetUsersAsync()
        {
            var items = await _repository.GetUsersAsync().ConfigureAwait(false);
            if (items == null)
            {
                return NotFound();
            }
            return Ok(items);
        }

        [HttpGet("info")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUsersInfoAsync()
        {
            var items = await _repository.GetUsersInfoAsync().ConfigureAwait(false);
            if (items == null)
            {
                return NotFound();
            }
            return Ok(items);
        }
    }
}