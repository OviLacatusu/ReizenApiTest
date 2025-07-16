using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp1.Components.Account.Pages
{
    [Authorize]
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGet ()
        {
            //var authenticationProperties = new LogoutAuthenticationPropertiesBuilder ()
            //     .WithRedirectUri ("/")
            //.Build ();

            await HttpContext.SignOutAsync (OpenIdConnectDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync (CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect ("/");
        }
    }
}
