using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace BlazorApp1.Models
{
    public class CustomAuthDelegatingHandler : DelegatingHandler
    {
        IHttpContextAccessor _httpContextAccessor;
        public CustomAuthDelegatingHandler (IHttpContextAccessor context)
        {
            InnerHandler = new HttpClientHandler ();
            _httpContextAccessor = context;
        }

        protected override async Task<HttpResponseMessage> SendAsync (
          HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri?.ToString().ToLower().Contains("/api/test") is true && _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated is true)
            {
                var properties = _httpContextAccessor.HttpContext?.Features.Get<IAuthenticateResultFeature> ()?.AuthenticateResult?.Properties;
                var test = properties?.GetTokens ();
                var accessToken = test?.Where (e => e.Name.ToLower ().Contains ("access"))?.FirstOrDefault ()?.Value;

                if (accessToken is not null)
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue ("Bearer", accessToken);
                }
                
            }
            return await base.SendAsync(request,cancellationToken);
        }
    }
}
