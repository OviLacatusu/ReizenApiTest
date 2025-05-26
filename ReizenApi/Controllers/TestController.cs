using Azure.Core;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Gmail.v1;
using Google.Apis.Json;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using GoogleAccess.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Reizen.Domain.Models;
using System.Collections.Immutable;
using System.Net;

namespace ReizenApi.Controllers
{
    [Route ("api/[controller]")]
    public class TestController : Controller
    {
        private const string CODE_ARG = "code";
        private const string CREDENTIALS_ARG = "credentials";
        private const string SHEETSERVICE_ARG = "sheetService";
        private const string AUTHHELPER_ARG = "AuthResponseHelper";
        private const int REFRESH_INT_MILI = 10000;
        private const string GOOGLE_PHOTOS_API_URL = "https://photospicker.googleapis.com/v1/mediaItems";
        private const string GOOGLE_PICKER_API_SESSION_REQ = "https://photospicker.googleapis.com/v1/sessions";

        private static string[] scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        private ImmutableDictionary<string, KZJobEntry> kzEntries;

        private readonly GoogleAuthConfig _config;
        private readonly IHttpClientFactory _httpFactory;
        private readonly GoogleAuthService  _service;
        private readonly ILogger<TestController> _logger;

        public TestController (

            IHttpClientFactory _httpFactory,
            ILogger<TestController> _logger,
            GoogleAuthConfig _config,
            GoogleAuthService _service
        )
        {
            this._config = _config ?? throw new ArgumentNullException (nameof(_config));
            this._httpFactory = _httpFactory ?? throw new ArgumentNullException (nameof(_httpFactory));
            this._service = _service ?? throw new ArgumentNullException (nameof(_service));
            //this._session = _session ?? throw new ArgumentNullException (nameof (_session));
            this._logger = _logger ?? throw new ArgumentNullException (nameof(_logger));
        }

        [HttpGet ("GetAuthLink")]
        public async Task<ActionResult> GetLink (CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation ("Getting Google authorization link");
                var link = _service.GetAuthenticationUri ();
                var content = new IndexViewModel ();
                content.Uri = link;

                return Ok (content);
            }
            catch (Exception ex)
            {
                _logger.LogError ($"Error {ex.Message}");
                return StatusCode (500, ex);
            }
        }
        [HttpGet("HandleCallback")]
        public async Task<ActionResult> HandleCallback(CancellationToken cancellationToken, [FromQuery] string code) 
        {
            var test = code;
            try
            {
                _logger.LogInformation ("Handling callback serverside");
                var responseContent = (await _service.ExchangeAuthorizationCodeAsync (code));

                this.HttpContext.Session.Set<AuthResponse>("oauthResponse", responseContent);
                return Ok(JsonConvert.SerializeObject(responseContent.AccessToken));
            }
            catch (Exception ex) {
                _logger.LogError ($"Error {ex.Message}");
                return StatusCode(500, ex);
            }
        }

        [HttpGet ("GetOauthServerSettings")]
        public async Task<ActionResult> GetClientId (CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty (_config.ClientId))
                {
                    throw new ArgumentNullException (nameof (_config.ClientId));
                }
                return Ok (_config.ClientId);
            }
            catch (Exception ex) 
            {
                _logger.LogError ($"Error {ex.Message}");
                return StatusCode (500, ex);
            }
        }
        [HttpGet ("CopyPhoto/{id}")]
        public async Task<ActionResult> CopyPhoto (string id, CancellationToken cancellationToken)
        {
            try
            {
                // restore AuthResponse state from session
                var oauth = this.HttpContext.Session.Get<AuthResponse> ("oauthResponse");
                if (oauth == null)
                    throw new OAuth2Exception ("Access token not found!");
                if (String.IsNullOrEmpty (id))
                    throw new ArgumentException ("Provided id is null or the empty string!");

                using (var client = _httpFactory.CreateClient ())
                {
                    // setting the Authentication header necessary for the request
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue ("Bearer", oauth.AccessToken);
                    
                    var result = await client.GetAsync ($"{GOOGLE_PHOTOS_API_URL}/{id}");

                    result.EnsureSuccessStatusCode ();
                    var details = JsonConvert.DeserializeObject<MediaFile> (await result.Content.ReadAsStringAsync ());

                    var bytes = await client.GetByteArrayAsync ($"{details.baseUrl}=d");
                    await System.IO.File.WriteAllBytesAsync (details.filename, bytes);
                    return Ok ("Image copyed");
                }
            }
            catch (Exception ex) {
                _logger.LogError ($"Error {ex.Message}");
                return StatusCode (500, ex.Message);
            }
        }
        [HttpGet("GetFiles")]
        public async Task<ActionResult> GetListFiles([FromHeader] string Authorization,CancellationToken cancellationToken)
        {
            try
            {
                // restore AuthResponse state from session
                //var oauth = this.HttpContext.Session.Get<AuthResponse> ("oauthResponse");
                if (Authorization == null)
                    throw new OAuth2Exception ("Access token not found!");
                var accessToken = Authorization.Split(" ").Last ();
                // creating credentials from access token, necessary for the DriveService creation
                GoogleCredential credentials = GoogleCredential.FromAccessToken (accessToken);

                DriveService service = new DriveService (new BaseClientService.Initializer ()
                {
                    HttpClientInitializer = credentials,
                    ApplicationName = "Testing Drive",
                    ValidateParameters = false
                });
                // setting up the request
                var request = service.Files.List ();

                request.IncludeItemsFromAllDrives = true;
                request.PageSize = 200;
                request.AccessToken = accessToken;
                request.SupportsAllDrives = true;
                request.Corpora = "allDrives";

                request.Fields = "files(id, name, mimeType, parents, createdTime, webViewLink, fileExtension, size)";
                var result = await request.ExecuteAsync ();
                
                // TODO: replace the Anonymous type with a defined class
                return Ok (Json (result.Files.Select (el => new {Id = el.Id, Mime = el.MimeType, Name = el.Name, Label = el.LabelInfo })));
            }
            catch (Exception ex) {
                _logger.LogError ($"Error {ex.Message}");
                return StatusCode (500, ex.Message);
            }
        }

        [HttpGet ("GetPickerLink")]
        public async Task<ActionResult> GetPickerData ([FromHeader] string Authorization, [FromServices] IHttpContextAccessor context, CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine (context.HttpContext);
                // restore AuthResponse state from session
                
                if (Authorization  is null)
                    throw new OAuth2Exception ("Access token not found! Session has probably expired.");
                var accessToken = Authorization.Split(" ").Last ();

                using (var client = _httpFactory.CreateClient ())
                {
                    // setting the Authentication header necessary for the request
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue ("Bearer", accessToken);
                    // sending an empty PickingSession object that will be returned with session details
                    var httpContent = new StringContent(JsonConvert.SerializeObject(new PickingSession()));
                    // POST request
                    var response = await client.PostAsync ($"{GOOGLE_PICKER_API_SESSION_REQ}", httpContent);
                    response.EnsureSuccessStatusCode ();

                    var session = JsonConvert.DeserializeObject<PickingSession> (await response.Content.ReadAsStringAsync());

                    return Ok (session);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError ($"Error {ex.Message}");
                return StatusCode (500, ex);
            }
        }
        // Not supported since around 1/05/2025
        //[HttpGet ("GetPhotos2")]
        public async Task<ActionResult> GetListPhotos2 ([FromHeader] string Authorization, CancellationToken token)
        {
            try
            {
                
                // restore AuthResponse state from session
                //var oauth = this.HttpContext.Session.Get<AuthResponse> ("oauthResponse");

                if (Authorization == null)
                    throw new OAuth2Exception ("Access token not found! Session has probably expired.");
                var accessToken = Authorization.Split (" ").Last ();

                using (var client = _httpFactory.CreateClient ())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue ("Bearer", accessToken);

                    var urlRequest = GOOGLE_PHOTOS_API_URL;

                    List<MediaFile> files = new List<MediaFile> ();
                    Boolean hasNextPage = true;

                    var content = await SendGetRequest<DetailsFiles> (urlRequest, token, false, null);
                    //files.AddRange (content.mediaItems.ToList());

                    DateTime timeStart = DateTime.Now;
                    do
                    {
                        if (!String.IsNullOrEmpty (content.nextPageToken))
                        {
                            content = await SendGetRequest<DetailsFiles> ($"{urlRequest}?&pageToken={content.nextPageToken}", token, false, null);
                            //files.AddRange (content.mediaItems.ToList ());
                        }
                        else
                        {
                            hasNextPage = false;
                        }

                    } while (hasNextPage);
                    DateTime timeEnd = DateTime.Now;
                    files.Last ().filename += "   " + timeEnd.Subtract (timeStart).Seconds;

                    if (files.Count > 0)
                    {
                        return Ok (files);
                    }
                    else
                    {
                        return NotFound ("No media items found");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError ($"Error {ex.Message}");
                return StatusCode (500, ex);
            }
        }
        [HttpGet("GetPhotosWithPicker/{sessionId}")]
        public async Task<ActionResult> GetListWithPicker (string sessionId, [FromHeader] string Authorization, CancellationToken cancellationToken)
        {
            try
            {
                // pull access token from header
                if (Authorization == null)
                    throw new OAuth2Exception ("Access token not found! Session has probably expired.");
                var accessToken = Authorization.Split (" ").Last ();

                _logger.LogInformation ($"Running PollRequest at {DateTime.UtcNow.ToShortTimeString ()}");

                if (string.IsNullOrEmpty (sessionId))
                    throw new ArgumentException ($"Invalid argument: sessionId = {sessionId} ");

                var url = new Uri ($"https://photospicker.googleapis.com/v1/sessions/{sessionId}");

                // while loop for polling the session until mediaItemsSet is set to true
                while (!cancellationToken.IsCancellationRequested)
                {
                    _logger.LogInformation ($"Running PollRequest ->> while loop at {DateTime.UtcNow.ToShortTimeString ()}");
                    await Task.Delay (3000, cancellationToken);

                    var session = await SendGetRequest<PickingSession> (url.ToString(), cancellationToken, true, accessToken);

                    if (session?.mediaItemsSet == true)
                    {
                        break;
                    }
                }
                _logger.LogInformation ($"Exited while loop in {nameof(GetListWithPicker)} at {DateTime.UtcNow.ToShortTimeString ()}");
                // requesting the details of the media items chosen by the user 
                // TODO: Add support for pagination
                url = new Uri ($"https://photospicker.googleapis.com/v1/mediaItems?sessionId={sessionId}");

                var details = await SendGetRequest<DetailsFiles> (url.ToString(), cancellationToken, true, accessToken);

                return Ok (details);
            }
            catch (Exception ex) 
            {
                _logger.LogError ($"Error {ex.Message}");
                return StatusCode (500, ex);
            }
        }
        // Generic API request that returns deserialized JSON Objects
        private async Task<T?> SendGetRequest<T> (string url, CancellationToken stoppingToken, bool WithBearerHeader, string? accessToken)
        {
            try
            {
                if (WithBearerHeader && String.IsNullOrEmpty (accessToken))
                {
                    throw new ArgumentNullException ("Access token missing!");
                }
                using (var client = _httpFactory.CreateClient ())
                {
                    if (WithBearerHeader)
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue ("Bearer", accessToken);
                    }
                    var response = await client.GetAsync (url, stoppingToken);
                    var content = JsonConvert.DeserializeObject<T> (await response.Content.ReadAsStringAsync ());
                    return (content);
                }
            }
            catch (Exception ex) 
            {
                _logger.LogError ($"Error {ex.Message}");
                throw new HttpRequestException(ex.Message, ex.InnerException) ;
            }
        }
        [HttpGet ("GetEmails")]
        public async Task<ActionResult> GetEmails ([FromHeader] string Authorization, CancellationToken cancellationToken)
        {
            try
            {
                // restore AuthResponse state from session
                //var oauth = this.HttpContext.Session.Get<AuthResponse> ("oauthResponse");
                if (Authorization == null)
                    throw new OAuth2Exception ("Access token not found! Session has probably expired.");
                var accessToken = Authorization.Split(" ").Last ();

                var credentials = GoogleCredential.FromAccessToken (accessToken);
                var service = new GmailService (new BaseClientService.Initializer ()
                {
                    HttpClientInitializer = credentials,
                    ApplicationName = "Testing Drive",
                    ValidateParameters = false
                });
                // For testing purposes
                var request = service.Users.Messages.List("ovi.lacatusu@gmail.com");
                request.LabelIds = "INBOX";
                request.IncludeSpamTrash = false;
                request.AccessToken = accessToken;

                var result = await request.ExecuteAsync ();
                return Ok (result.Messages);
            }
            catch (Exception ex) {
                _logger.LogError ($"Error {ex.Message}");
                return StatusCode (500, ex);
            }
        }
    }
}