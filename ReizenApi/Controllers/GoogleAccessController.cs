using Azure.Core;
using Azure.Messaging;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Json;
using Google.Apis.Requests;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util;
using GoogleAccess.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Collections.Immutable;
using System.Net;
using System.Web;

namespace ReizenApi.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class GoogleAccessController : ControllerBase
    {
        private const string GOOGLE_PHOTOS_API_URL = "https://photoslibrary.googleapis.com/v1/mediaItems";
        private const string GOOGLE_PICKER_API_SESSION_REQ = "https://photospicker.googleapis.com/v1/sessions";
        private const string GOOGLE_GMAIL_AUTHENTICATED_USER_URL = "https://www.googleapis.com/gmail/v1/users/me/profile";
        private const string GOOGLE_PICKER_PHOTOS_REQ = "https://photospicker.googleapis.com/v1/mediaItems";
        private const int MAX_POLLING_ATTEMPTS = 20; // 1 minute max polling
        private const int POLLING_INTERVAL_MS = 3000;

        private readonly IHttpClientFactory _httpFactory;
        private readonly ILogger<GoogleAccessController> _logger;

        public GoogleAccessController (
            IHttpClientFactory _httpFactory,
            ILogger<GoogleAccessController> _logger
        )
        {
            this._httpFactory = _httpFactory ?? throw new ArgumentNullException (nameof (_httpFactory));
            this._logger = _logger ?? throw new ArgumentNullException (nameof (_logger));
        }

        [HttpGet ("DownloadFoto/{encodedUrl}/{encodedMimeType}")]
        public async Task<IActionResult> DownloadFotoAsync ([FromHeader] string Authorization, string encodedUrl, string encodedMimeType, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty (Authorization))
                    return Unauthorized ("Access token not found!");

                if (string.IsNullOrEmpty (encodedUrl))
                    return BadRequest ("URL parameter is required");

                if (string.IsNullOrEmpty (encodedMimeType))
                    return BadRequest ("MIME type parameter is required");

                var baseUrl = HttpUtility.UrlDecode (encodedUrl);
                var mimeType = HttpUtility.UrlDecode (encodedMimeType);
                var accessToken = ExtractAccessToken (Authorization);

                using var client = _httpFactory.CreateClient ();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue ("Bearer", accessToken);

                var stream = await client.GetStreamAsync ($"{baseUrl}=d", cancellationToken);
                var result = new FileStreamResult (stream, mimeType);

                return result;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError (ex, "HTTP error in DownloadFotoAsync");
                return StatusCode (502, "Failed to download file from Google Photos");
            }
            catch (Exception ex)
            {
                _logger.LogError (ex, "Unexpected error in DownloadFotoAsync");
                return StatusCode (500, "An unexpected error occurred");
            }
        }
        // TO DO
        [HttpGet ("GetFiles")]
        public async Task<ActionResult> GetListFilesAsync ([FromHeader] string Authorization, CancellationToken cancellationToken)
        {
            try
            {
                if (Authorization == null)
                    throw new OAuth2Exception ("Access token not found!");
                var accessToken = Authorization.Split (" ").Last ();
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
                return Ok (JsonConvert.SerializeObject (result.Files.Select (el => new { Id = el.Id, Mime = el.MimeType, Name = el.Name, Label = el.LabelInfo })));
            }
            catch (Exception ex)
            {
                _logger.LogError ($"Error {ex.Message}");
                return StatusCode (500, ex.Message);
            }
        }
        // Method returning a link for the generated picker session. Once accessed, the link becomes invalid
        [HttpGet ("GetPickerLink")]
        public async Task<ActionResult> GetPickerDataAsync ([FromHeader] string Authorization, [FromServices] IHttpContextAccessor context, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty (Authorization))
                    return Unauthorized ("Access token not found! Session has probably expired.");

                var accessToken = ExtractAccessToken (Authorization);

                using var client = _httpFactory.CreateClient ();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue ("Bearer", accessToken);

                var httpContent = new StringContent (JsonConvert.SerializeObject (new PickingSession ()));
                var response = await client.PostAsync (GOOGLE_PICKER_API_SESSION_REQ, httpContent, cancellationToken);
                response.EnsureSuccessStatusCode ();

                var session = JsonConvert.DeserializeObject<PickingSession> (await response.Content.ReadAsStringAsync ());
                return Ok (session);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError (ex, "HTTP error in GetPickerDataAsync");
                return StatusCode (502, "Failed to create picker session");
            }
            catch (Exception ex)
            {
                _logger.LogError (ex, "Unexpected error in GetPickerDataAsync");
                return StatusCode (500, "An unexpected error occurred");
            }
        }

        private static string ExtractAccessToken (string authorization)
        {
            if (string.IsNullOrEmpty (authorization))
                throw new ArgumentException ("Authorization header is empty");

            var parts = authorization.Split (' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2 || !string.Equals (parts[0], "Bearer", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException ("Invalid authorization header format. Expected 'Bearer <token>'");

            return parts[1];
        }
        // This approach is not supported since around 1/05/2025
        //[HttpGet ("GetPhotos2")]
        public async Task<ActionResult> GetListPhotos2Async ([FromHeader] string Authorization, CancellationToken token)
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

                    var content = await SendGetRequestAsync<GPhotosDetailsFiles> (urlRequest, token, false, null);
                    //files.AddRange (content.mediaItems.ToList());

                    DateTime timeStart = DateTime.Now;
                    do
                    {
                        if (!String.IsNullOrEmpty (content.NextPageToken))
                        {
                            content = await SendGetRequestAsync<GPhotosDetailsFiles> ($"{urlRequest}?&pageToken={content.NextPageToken}", token, false, null);
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
        [HttpGet ("GetPhotosWithPicker/{sessionId}")]
        public async Task<ActionResult> GetListWithPickerAsync (string sessionId, [FromHeader] string Authorization, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty (Authorization))
                    return Unauthorized ("Access token not found! Session has probably expired.");

                if (string.IsNullOrEmpty (sessionId))
                    return BadRequest ("Session ID is required");

                var accessToken = ExtractAccessToken (Authorization);
                _logger.LogInformation ("Starting polling for session {SessionId}", sessionId);

                var url = new Uri ($"{GOOGLE_PICKER_API_SESSION_REQ}/{sessionId}");
                var pollingAttempts = 0;

                // Polling loop with timeout protection
                while (pollingAttempts < MAX_POLLING_ATTEMPTS && !cancellationToken.IsCancellationRequested)
                {
                    _logger.LogDebug ("Polling attempt {Attempt} for session {SessionId}", pollingAttempts + 1, sessionId);

                    await Task.Delay (POLLING_INTERVAL_MS, cancellationToken);

                    var session = await SendGetRequestAsync<PickingSession> (url.ToString (), cancellationToken, true, accessToken);

                    if (session?.mediaItemsSet == true)
                    {
                        _logger.LogInformation ("Session {SessionId} completed after {Attempts} attempts", sessionId, pollingAttempts + 1);
                        break;
                    }

                    pollingAttempts++;
                }

                if (pollingAttempts >= MAX_POLLING_ATTEMPTS)
                {
                    _logger.LogWarning ("Session {SessionId} timed out after {Attempts} attempts", sessionId, MAX_POLLING_ATTEMPTS);
                    return StatusCode (408, "Session polling timed out");
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    _logger.LogInformation ("Session {SessionId} polling was cancelled", sessionId);
                    return StatusCode (499, "Request was cancelled");
                }

                // Request the details of the media items chosen by the user
                var detailsUrl = new Uri ($"{GOOGLE_PICKER_PHOTOS_REQ}?sessionId={sessionId}");
                var details = await SendGetRequestAsync<GPhotosDetailsFiles> (detailsUrl.ToString (), cancellationToken, true, accessToken);

                return Ok (details);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError (ex, "HTTP error in GetListWithPickerAsync for session {SessionId}", sessionId);
                return StatusCode (502, "Failed to communicate with Google Photos API");
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation ("Operation was cancelled for session {SessionId}", sessionId);
                return StatusCode (499, "Request was cancelled");
            }
            catch (Exception ex)
            {
                _logger.LogError (ex, "Unexpected error in GetListWithPickerAsync for session {SessionId}", sessionId);
                return StatusCode (500, "An unexpected error occurred");
            }
        }
        // Generic API GET request that returns deserialized JSON Objects
        private async Task<T?> SendGetRequestAsync<T> (string url, CancellationToken cancellationToken, bool withBearerHeader, string? accessToken)
        {
            try
            {
                if (withBearerHeader && string.IsNullOrEmpty (accessToken))
                {
                    throw new ArgumentException ("Access token is required when withBearerHeader is true");
                }

                using var client = _httpFactory.CreateClient ();

                if (withBearerHeader)
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue ("Bearer", accessToken);
                }

                var response = await client.GetAsync (url, cancellationToken);
                response.EnsureSuccessStatusCode ();

                var content = await System.Text.Json.JsonSerializer.DeserializeAsync<T> (await response.Content.ReadAsStreamAsync ());
                return content;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError (ex, "HTTP error in SendGetRequestAsync for URL: {Url}", url);
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError (ex, "JSON deserialization error in SendGetRequestAsync for URL: {Url}", url);
                throw new HttpRequestException ("Failed to parse response", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError (ex, "Unexpected error in SendGetRequestAsync for URL: {Url}", url);
                throw new HttpRequestException ("An unexpected error occurred", ex);
            }
        }

        // Method retrieving gmail messages of the currently authenticated with Google user
        [HttpGet ("GetMessages")]
        public async Task<ActionResult> GetMessagesAsync ([FromHeader] string Authorization, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty (Authorization))
                    return Unauthorized ("Access token not found! Session has probably expired.");

                var accessToken = ExtractAccessToken (Authorization);
                var credentials = GoogleCredential.FromAccessToken (accessToken);

                var service = new GmailService (new BaseClientService.Initializer ()
                {
                    HttpClientInitializer = credentials,
                    ApplicationName = "Testing Gmail",
                    ValidateParameters = false
                });

                using var client = _httpFactory.CreateClient ();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue ("Bearer", accessToken);

                // Get user profile
                var response = await client.GetAsync (GOOGLE_GMAIL_AUTHENTICATED_USER_URL, cancellationToken);
                response.EnsureSuccessStatusCode ();

                var profile = JsonConvert.DeserializeObject<GmailUserProfile> (await response.Content.ReadAsStringAsync ());
                if (profile?.EmailAddress == null)
                {
                    return BadRequest ("Unable to retrieve user email address");
                }

                // Get message list
                var request = service.Users.Messages.List (profile.EmailAddress);
                request.IncludeSpamTrash = false;
                request.AccessToken = accessToken;
                request.MaxResults = 50;

                var result = await request.ExecuteAsync (cancellationToken);
                var listMessages = new List<Message> ();

                var iterationCount = 0;
                const int maxIterations = 2; // Limit to prevent excessive API calls

                do
                {
                    if (result.Messages == null || !result.Messages.Any ())
                        break;

                    var batch = new BatchRequest (service);
                    var batchTasks = new List<Task> ();

                    foreach (var elem in result.Messages)
                    {
                        var request2 = service.Users.Messages.Get (profile.EmailAddress, elem.Id);
                        batch.Queue<Message> (request2, (content, error, i, message) =>
                        {
                            if (error == null)
                            {
                                listMessages.Add (content);
                            }
                            else
                            {
                                _logger.LogWarning ("Error retrieving message {MessageId}: {Error}", elem.Id, error.Message);
                            }
                        });
                    }

                    await batch.ExecuteAsync (cancellationToken);

                    if (string.IsNullOrEmpty (result.NextPageToken) || iterationCount >= maxIterations)
                        break;

                    request.PageToken = result.NextPageToken;
                    result = await request.ExecuteAsync (cancellationToken);
                    iterationCount++;
                } while (true);

                return Ok (listMessages);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError (ex, "HTTP error in GetMessagesAsync");
                return StatusCode (502, "Failed to communicate with Gmail API");
            }
            catch (Exception ex)
            {
                _logger.LogError (ex, "Unexpected error in GetMessagesAsync");
                return StatusCode (500, "An unexpected error occurred");
            }
        }

        [HttpGet ("GetMessage/{messageId}")]
        public async Task<ActionResult> GetMessageAsync ([FromHeader] string Authorization, string messageId, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty (Authorization))
                    return Unauthorized ("Access token not found! Session has probably expired.");

                if (string.IsNullOrEmpty (messageId))
                    return BadRequest ("Message ID is required");

                var accessToken = ExtractAccessToken (Authorization);
                var credentials = GoogleCredential.FromAccessToken (accessToken);

                var service = new GmailService (new BaseClientService.Initializer ()
                {
                    HttpClientInitializer = credentials,
                    ApplicationName = "Testing Gmail",
                    ValidateParameters = false
                });

                using var client = _httpFactory.CreateClient ();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue ("Bearer", accessToken);

                // Get user profile
                var response = await client.GetAsync (GOOGLE_GMAIL_AUTHENTICATED_USER_URL, cancellationToken);
                response.EnsureSuccessStatusCode ();

                var profile = JsonConvert.DeserializeObject<GmailUserProfile> (await response.Content.ReadAsStringAsync ());
                if (profile?.EmailAddress == null)
                {
                    return BadRequest ("Unable to retrieve user email address");
                }

                var request = service.Users.Messages.Get (profile.EmailAddress, messageId);
                request.Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Raw;
                request.AccessToken = accessToken;

                var result = await request.ExecuteAsync (cancellationToken);
                return Ok (result);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError (ex, "HTTP error in GetMessageAsync for message {MessageId}", messageId);
                return StatusCode (502, "Failed to communicate with Gmail API");
            }
            catch (Exception ex)
            {
                _logger.LogError (ex, "Unexpected error in GetMessageAsync for message {MessageId}", messageId);
                return StatusCode (500, "An unexpected error occurred");
            }
        }

        [HttpPost ("DeleteMessage")]
        public async Task<ActionResult> DeleteMessageAsync ([FromHeader] string Authorization, [FromBody] string messageId, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty (Authorization))
                    return Unauthorized ("Access token not found! Session has probably expired.");

                if (string.IsNullOrEmpty (messageId))
                    return BadRequest ("Message ID is required");

                var accessToken = ExtractAccessToken (Authorization);
                var credentials = GoogleCredential.FromAccessToken (accessToken);

                var service = new GmailService (new BaseClientService.Initializer ()
                {
                    HttpClientInitializer = credentials,
                    ValidateParameters = false
                });

                using var client = _httpFactory.CreateClient ();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue ("Bearer", accessToken);

                var response = await client.GetAsync (GOOGLE_GMAIL_AUTHENTICATED_USER_URL, cancellationToken);
                response.EnsureSuccessStatusCode ();

                var profile = await response.Content.ReadFromJsonAsync<GmailUserProfile> (cancellationToken: cancellationToken);
                if (profile?.EmailAddress == null)
                {
                    return BadRequest ("Unable to retrieve user email address");
                }

                var request = service.Users.Messages.Delete (profile.EmailAddress, messageId);
                await request.ExecuteAsync (cancellationToken);

                return NoContent ();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError (ex, "HTTP error in DeleteMessageAsync for message {MessageId}", messageId);
                return StatusCode (502, "Failed to communicate with Gmail API");
            }
            catch (Exception ex)
            {
                _logger.LogError (ex, "Unexpected error in DeleteMessageAsync for message {MessageId}", messageId);
                return StatusCode (500, "An unexpected error occurred");
            }
        }
    }
}