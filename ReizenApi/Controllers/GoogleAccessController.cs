using Azure.Core;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Gmail.v1;
using Google.Apis.Json;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util;
using GoogleAccess.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Immutable;
using System.Net;
using System.Web;

namespace ReizenApi.Controllers
{
    [Route ("api/[controller]")]
    public class GoogleAccessController : Controller
    {
        private const string GOOGLE_PHOTOS_API_URL = "https://photoslibrary.googleapis.com/v1/mediaItems";
        private const string GOOGLE_PICKER_API_SESSION_REQ = "https://photospicker.googleapis.com/v1/sessions";

        private static string[] scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        private ImmutableDictionary<string, KZJobEntry> kzEntries;

        private readonly IHttpClientFactory _httpFactory;
        private readonly ILogger<GoogleAccessController> _logger;

        public GoogleAccessController (
            IHttpClientFactory _httpFactory,
            ILogger<GoogleAccessController> _logger
        )
        {
            this._httpFactory = _httpFactory ?? throw new ArgumentNullException (nameof(_httpFactory));
            this._logger = _logger ?? throw new ArgumentNullException (nameof(_logger));
        }

        [HttpGet ("DownloadFoto/{encodedUrl}/{encodedMimeType}")]
        public async Task<IActionResult> DownloadFoto ([FromHeader] string Authorization, string encodedUrl, string encodedMimeType, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation ($"Authorization: {Authorization} ");
                // restore AuthResponse state from session
                if (Authorization == null)
                    throw new OAuth2Exception ("Access token not found!");
                if (String.IsNullOrEmpty (encodedUrl))
                    throw new ArgumentException ("Provided id is null or the empty string!");
                var baseurl = HttpUtility.UrlDecode (encodedUrl);
                var mimeType = HttpUtility.UrlDecode (encodedMimeType);
                var accessToken = Authorization.Split (" ").Last ();
                using (var client = _httpFactory.CreateClient ())
                {
                    _logger.LogInformation ($"Base url : {baseurl}");
                    _logger.LogInformation ($"access token: {Authorization}");
                    // setting the Authentication header necessary for the request
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue ("Bearer", accessToken);
                    
                    var stream = await client.GetStreamAsync($"{baseurl}=d");
                    var result = new FileStreamResult(stream, mimeType);

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError ($"Error {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("GetFiles")]
        public async Task<ActionResult> GetListFiles([FromHeader] string Authorization,CancellationToken cancellationToken)
        {
            try
            {
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
        // Methor returning a link for the generated picker session. Once accessed link becomes invalid
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
                if (Authorization == null)
                    throw new OAuth2Exception ("Access token not found! Session has probably expired.");
                var accessToken = Authorization.Split (" ").Last ();

                using (var client = _httpFactory.CreateClient ())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue ("Bearer", accessToken);

                    var urlRequest = GOOGLE_PHOTOS_API_URL;

                    List<MediaFile> files = new List<MediaFile> ();
                    Boolean hasNextPage = true;

                    var content = await SendGetRequest<GPhotosDetailsFiles> (urlRequest, token, false, null);
                    //files.AddRange (content.mediaItems.ToList());

                    DateTime timeStart = DateTime.Now;
                    do
                    {
                        if (!String.IsNullOrEmpty (content.NextPageToken))
                        {
                            content = await SendGetRequest<GPhotosDetailsFiles> ($"{urlRequest}?&pageToken={content.NextPageToken}", token, false, null);
                            //files.AddRange (content.mediaItems.ToList ());
                        }
                        else
                        {
                            hasNextPage = false;
                        }

                    } while (hasNextPage);
                    DateTime timeEnd = DateTime.Now;
                    files.Last ().Filename += "   " + timeEnd.Subtract (timeStart).Seconds;

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

                var details = await SendGetRequest<GPhotosDetailsFiles> (url.ToString(), cancellationToken, true, accessToken);

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
        [HttpGet ("GetMessages")]
        public async Task<ActionResult> GetMessages([FromHeader] string Authorization, CancellationToken cancellationToken)
        {
            try
            {
                if (Authorization == null)
                    throw new OAuth2Exception ("Access token not found! Session has probably expired.");

                var accessToken = Authorization.Split(" ").Last ();

                var credentials = GoogleCredential.FromAccessToken (accessToken);
                var service = new GmailService (new BaseClientService.Initializer ()
                {
                    HttpClientInitializer = credentials,
                    ApplicationName = "Testing Gmail",
                    ValidateParameters = false
                });
                // For testing purposes
                using var client = _httpFactory.CreateClient ();
                {
                    var response = await client.GetAsync("https://www.googleapis.com/gmail/v1/users/me/profile"); 
                    response.EnsureSuccessStatusCode ();
                    var profile = await response.Content.ReadFromJsonAsync<GmailUserProfile> ();

                    var request = service.Users.Messages.List(profile?.EmailAddress);

                    request.LabelIds = "INBOX";
                    request.IncludeSpamTrash = false;
                    request.AccessToken = accessToken;
                    var result = await request.ExecuteAsync ();
                    
                    return Ok (result.Messages);
                }
                
            }
            catch (Exception ex) {
                _logger.LogError ($"Error {ex.Message}"); 
                return StatusCode (500, ex);
            }
        }
        [HttpPost ("DeleteMessage")]
        public async Task<ActionResult> DeleteMessage ([FromHeader] string Authorization, [FromBody] string messageId, CancellationToken cancellationToken)
        {
            try
            {
                if (Authorization == null)
                    throw new OAuth2Exception ("Access token not found! Session has probably expired.");

                var accessToken = Authorization.Split (" ").Last ();
                var credentials = GoogleCredential.FromAccessToken (accessToken);

                var service = new GmailService ( new BaseClientService.Initializer ()
                {   
                    HttpClientInitializer = credentials,
                    ValidateParameters = false
                });

                using var client = _httpFactory.CreateClient ();
                {
                    var response = await client.GetAsync ("https://www.googleapis.com/gmail/v1/users/me/profile");
                    response.EnsureSuccessStatusCode ();
                    var profile = await response.Content.ReadFromJsonAsync<GmailUserProfile> ();

                    var request = service.Users.Messages.Delete (profile?.EmailAddress, messageId);
                    return NoContent();
                }
            }
            catch (Exception ex) 
            {
                _logger.LogError ($"Error {ex.Message}");
                return StatusCode(500, ex);
            }
        }
    }
}