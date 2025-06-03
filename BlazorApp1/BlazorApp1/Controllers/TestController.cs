using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp1.Controllers
{
    [Route ("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet ("signin-google")]
        public ActionResult Login (string returnUrl)
        {
            return Challenge (new AuthenticationProperties
            {
                RedirectUri = !string.IsNullOrEmpty (returnUrl) ? returnUrl : "/"
            });
        }
    }
}
