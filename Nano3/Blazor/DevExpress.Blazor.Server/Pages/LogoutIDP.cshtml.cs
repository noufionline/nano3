using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevExpress.Blazor.Server.Pages
{
    public class LogoutIDPModel : PageModel
    {
        public async Task OnGetAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
             var prop = new AuthenticationProperties()
            {
                RedirectUri = "https://localhost:44314"
            };
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme,prop);
        }
    }
}