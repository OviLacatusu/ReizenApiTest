using BlazorApp1.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace BlazorApp1.Models
{
    public class CustomAuthDelegatingHandler : DelegatingHandler
    {
        private IHttpContextAccessor _httpContextAccessor;
        private UserManager<ApplicationUser> _userManager;
        public CustomAuthDelegatingHandler (IHttpContextAccessor context, UserManager<ApplicationUser> userManager)
        {
            InnerHandler = new HttpClientHandler ();
            _httpContextAccessor = context;
            _userManager = userManager;
        }

        protected override async Task<HttpResponseMessage> SendAsync (
          HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri?.ToString ().ToLower ().Contains ("/api/googleaccess") is true && _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated is true)
            {
                var currentUser = await _userManager.GetUserAsync (_httpContextAccessor.HttpContext.User);
                var accessToken = await _userManager.GetAuthenticationTokenAsync (currentUser, "GoogleOpenIdConnect", "access_token");

                if (accessToken is not null)
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue ("Bearer", accessToken);
                }
            }
            return await base.SendAsync (request, cancellationToken);
        }
    }
}
