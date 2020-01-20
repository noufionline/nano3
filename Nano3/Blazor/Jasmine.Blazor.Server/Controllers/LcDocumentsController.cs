using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using IdentityModel.Client;
using Jasmine.Blazor.Server.Pages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jasmine.Blazor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LcDocumentsController : ControllerBase
    {
        private readonly ILcDocumentService _client;

        public LcDocumentsController(ILcDocumentService client)
        {
            _client = client;
        }


        [HttpGet]
        public async  Task<ActionResult<List<LcDocumentList>>> GetCustomers()
        {
            HttpContext.GetUserAccessTokenAsync();
            return Ok(await _client.GetDocumentsAsync());
        }
    }
}