using Google.Apis.Gmail.v1.Data;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace BlazorApp1.Models
{
    public class ClaimsReporter
    {
        private RequestDelegate next;
        private ILogger<ClaimsReporter> _logger;

        public ClaimsReporter (RequestDelegate requestDelegate, ILogger<ClaimsReporter> logger)
        {
            next = requestDelegate;
            _logger = logger;
        }
        public async Task Invoke (HttpContext context)
        {
            ClaimsPrincipal p = context.User;
            _logger.LogInformation ($"\tUser: {p.Identity.Name}");
            _logger.LogInformation ($"\tAuthenticated: {p.Identity.IsAuthenticated}");
            _logger.LogInformation ($"\tAuthentication Type  {p.Identity.AuthenticationType}");
            _logger.LogInformation ($"\tIdentities: {p.Identities.Count ()}");
            foreach (ClaimsIdentity ident in p.Identities)
            {
                _logger.LogInformation ($"\tAuth type: {ident.AuthenticationType}, {ident.Claims.Count ()} claims");
                foreach (Claim claim in ident.Claims)
                {
                    _logger.LogInformation ($"\tType: {claim.Type}, Value: {claim.Value}, Issuer: {claim.Issuer}");
                }
            }
            await next (context);
        }
        private string? GetName (string claimType) =>
        Path.GetFileName (new Uri (claimType).LocalPath);
    }
}
