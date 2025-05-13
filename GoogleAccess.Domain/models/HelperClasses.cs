using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Threading.Tasks;
using System.Collections;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Text;

namespace GoogleAccess.Domain.Models
{
    public class KZJobEntry
    {
        public string JobName { get; set; }
        public string PrepAreaName { get; set; }
        public Color ColorBackground { get; set; }
    }

    public class GoogleAuthConfig
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string SpreadsheetId { get; set; }
        public string[] Ranges { get; set; }
        public string[] AuthScope { get; set; }
        public string AuthAccessType { get; set; }
        public string AuthRedirectUrl { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string ClientSecretPath{ get; set; }
    }

    public class GoogleAuthService 
    {
        public readonly GoogleAuthConfig _config;
        private readonly IHttpClientFactory _httpFactory;

        [JsonConstructor]
        public GoogleAuthService ()
        {
        }

        public GoogleAuthService(GoogleAuthConfig config, IHttpClientFactory httpClient)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _httpFactory = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public Uri GetAuthenticationUri()
        {
            var scopes = _config.AuthScope;
            var redirectUri = string.IsNullOrEmpty(_config.AuthRedirectUrl) 
                ? "urn:ietf:wg:oauth:2.0:oob" 
                :(_config.AuthRedirectUrl);

            var oauth = string.Format(
                "https://accounts.google.com/o/oauth2/v2/auth?&response_type=code&access_type={0}&prompt=consent&client_id={1}&redirect_uri={2}&scope={3}",
                _config.AuthAccessType,
                _config.ClientId,
                redirectUri,
                String.Join(" ",scopes));

            return new Uri(oauth);
        }

        public async Task<AuthResponse> ExchangeAuthorizationCodeAsync(string authCode)
        {
            var postData = new Dictionary<string, string>
            {
                { "code", authCode },
                { "client_id", _config.ClientId },
                { "client_secret", _config.ClientSecret },
                { "redirect_uri", _config.AuthRedirectUrl },
                { "grant_type", "authorization_code" }
            };

            var response = await SendTokenRequestAsync(postData);
            //response.ClientId = _config.ClientId;
            //response.Secret = _config.ClientSecret;

            return response;
        }
        public async Task<AuthResponse> RefreshTokenAsync ()
        {
            var postData = new Dictionary<string, string>
            {
                { "client_id", _config.ClientId },
                { "client_secret", _config.ClientSecret },
                { "refresh_token", _config.RefreshToken },
                { "grant_type", "refresh_token" }
            };
            return await SendTokenRequestAsync (postData);
        }

        private async Task<AuthResponse> SendTokenRequestAsync(Dictionary<string, string> postData)
        {
            var content = new FormUrlEncodedContent(postData);

            using (var httpClient = _httpFactory.CreateClient ())
            {
            
                var response = (await httpClient.PostAsync ("https://accounts.google.com/o/oauth2/token", content));
                response.EnsureSuccessStatusCode ();

                var responseString = await response.Content.ReadAsStringAsync ();
                var authResponseData = JsonConvert.DeserializeObject<AuthResponseData> (responseString);
                
                var authResponse = new AuthResponse (this)
                {
                    AccessToken = authResponseData.access_token,
                    RefreshToken = authResponseData.refresh_token,
                    ClientId =  _config.ClientId,
                    ExpiresIn = authResponseData.expires_in,
                    Created = DateTime.UtcNow,
                    Secret = _config.ClientSecret, 
                };
                return authResponse;
            }
        }
    }
    public record AuthResponseData(string access_token, string refresh_token, int expires_in);
    public class AuthResponse
    {
        
        private readonly GoogleAuthService _authService;
        private string _accessToken;
        public string AccessToken
        {
            get
            {
                if (DateTime.UtcNow.Subtract(Created).TotalHours >= 1)
                {
                    RefreshAsync().GetAwaiter().GetResult();
                }
                return _accessToken;
            }
            set => _accessToken = value;
        }

        public string RefreshToken { get; set; }
        public string ClientId { get; set; }
        public string Secret { get; set; }
        public int ExpiresIn { get; set; }
        public DateTime Created { get; set; }

        public AuthResponse(GoogleAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }
        [JsonConstructor]
        public AuthResponse() { }
        private async Task RefreshAsync()
        {
            var response = await _authService.RefreshTokenAsync();
            _accessToken = response.AccessToken;
            Created = DateTime.UtcNow;
        }
    }

    public interface IHttpClient
    {
        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);
    }

    public class HttpClientWrapper : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public HttpClientWrapper()
        {
            _httpClient = new HttpClient();
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            return await _httpClient.PostAsync(requestUri, content);
        }
    }
}
