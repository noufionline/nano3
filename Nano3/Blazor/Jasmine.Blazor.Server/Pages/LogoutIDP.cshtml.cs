using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;

namespace Jasmine.Blazor.Server.Pages
{
    public class LogoutIDPModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;

        public LogoutIDPModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public async Task OnGetAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var prop = new AuthenticationProperties()
            {
                RedirectUri = _environment.IsDevelopment() ? "https://localhost:44309" : "https://abs.cicononline.com/kpi"
            };
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, prop);
        }
    }
}