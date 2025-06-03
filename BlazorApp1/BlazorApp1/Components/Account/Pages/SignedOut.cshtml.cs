using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlazorApp1.Components.Account.Pages
{
    [AllowAnonymous]
    public class SignedOutModel : PageModel
    {
        public void OnGet ()
        {
        }
    }
}
