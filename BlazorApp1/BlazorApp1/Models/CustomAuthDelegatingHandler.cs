using Microsoft.AspNetCore.Authentication;

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
            if (request.RequestUri.AbsoluteUri.ToString().ToLower().Contains("/api/test") && _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true)
            {
                var properties = _httpContextAccessor.HttpContext?.Features.Get<IAuthenticateResultFeature> ()?.AuthenticateResult?.Properties;
                var test = properties?.GetTokens ();
                var accessToken = test?.Where (e => e.Name.ToLower ().Contains ("access"))?.FirstOrDefault ()?.Value;

                if (request.Method == HttpMethod.Get )
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue ("Bearer", accessToken);
                }
                
            }
            return await base.SendAsync(request,cancellationToken);
        }
    }
}
