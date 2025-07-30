using BlazorApp1.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Options;
using Reizen.CommonClasses;

namespace BlazorApp1.Models
{
    public class CustomAuthDelegatingHandler : DelegatingHandler
    {
        private IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ConfigOptions _config;
        private ILogger<CustomAuthDelegatingHandler> _logger;
        public CustomAuthDelegatingHandler (IHttpContextAccessor context, UserManager<ApplicationUser> userManager, IOptionsSnapshot<ConfigOptions> config, ILogger<CustomAuthDelegatingHandler> logger)
        {
            _httpContextAccessor = context;
            _userManager = userManager;
            _config = config.Value;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync (HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true)
            {
                var currentUser = await _userManager.GetUserAsync (_httpContextAccessor.HttpContext.User);
                var accessToken = await _userManager.GetAuthenticationTokenAsync (currentUser, _config.ExternalAuthMethod, "access_token");

                _logger.LogInformation ($"Executing the token injection request handler. User: {currentUser.UserName}");
                
                if (accessToken is not null)
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue ("Bearer", accessToken);
                }
            }
            return await base.SendAsync (request, cancellationToken);

        }
    }
}
