using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Google.Apis.Auth.AspNetCore3;

namespace BlazorApp1.Components.Account.Pages
{
    [Authorize]
    public class LogoutModel : PageModel
    {
        public IActionResult OnGetAsync ()
        {
            return SignOut (new AuthenticationProperties
            {
                RedirectUri = "/SignedOut"
            },
            // Clear auth cookie
            CookieAuthenticationDefaults.AuthenticationScheme,
            // Redirect to OIDC provider signout endpoint
            GoogleOpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}
