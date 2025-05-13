using System;
using System.Net;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Web;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;
using System.IO ;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Newtonsoft.Json;
using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Data = Google.Apis.Sheets.v4.Data;
using System.Drawing;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Google.Apis.Auth.OAuth2.Responses;
using System.Reflection;
using Microsoft.AspNetCore.Http.HttpResults;
using static System.Net.WebRequestMethods;
using Google.Apis.Gmail.v1;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Runtime.InteropServices;
using System.CodeDom;
using System.Collections.Concurrent;
using Google.Apis.Drive.v3.Data;
using GoogleAccess.Domain.Models;
using Reizen.Domain.Models;


namespace ReizenApi.Controllers
{
    [Route ("api/[controller]")]
    public class TestController2 : Controller
    {
        private const string CODE_ARG = "code";
        private const string CREDENTIALS_ARG = "credentials";
        private const string SHEETSERVICE_ARG = "sheetService";
        private const string AUTHHELPER_ARG = "AuthResponseHelper";
        private const int REFRESH_INT_MILI = 10000;
        private const string GOOGLE_PHOTOS_API_URL = "https://photoslibrary.googleapis.com/v1/mediaItems";

        private static string[] scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        private ImmutableDictionary<string, KZJobEntry> kzEntries;

        private readonly GoogleAuthConfig _config;
        private readonly IHttpClientFactory _httpFactory;
        private readonly GoogleAuthService  _service;

        public TestController2 (

            IHttpClientFactory _httpFactory,
            GoogleAuthConfig _config,
            GoogleAuthService _service
        )
        {
            this._config = _config ?? throw new ArgumentNullException (nameof(_config));
            this._httpFactory = _httpFactory ?? throw new ArgumentNullException (nameof(_httpFactory));
            this._service = _service ?? throw new ArgumentNullException (nameof(_service));
        }

        //[HttpGet]
        //[Route("/[controller]")]
        //[Route("/[controller]/[action]")]
        //public async Task<ActionResult> Index(CancellationToken cancellationToken)
        //{
        //    var link = _service.GetAuthenticationUri ();
        //    var content = new IndexViewModel ();
        //    content.Uri = (link);

        //    return View(content);

        //}
        //[HttpGet("HandleCallback")]
        public async Task<ActionResult> HandleCallback(CancellationToken cancellationToken, [FromQuery] string code) 
        {
            var test = code;
            var model = new IndexViewModel ();
            try
            {
                var responseContent = (await _service.ExchangeAuthorizationCodeAsync (code));
                this.HttpContext.Session.Set<AuthResponse> ("oauthResponse", responseContent);
               
            }
            catch (Exception ex) {
                model.Message += ex.Message + "\n " + ex.InnerException + "\n"+ex.StackTrace;
            }
            return RedirectToAction("index", model);

            throw new NotImplementedException ();
        }

        [HttpGet ("GetOauthServerSettings")]
        public async Task<ActionResult> GetClientId ()
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
                return StatusCode (500, ex);
            }
        }
        [HttpGet ("CopyPhoto/{id}")]
        public async Task<ActionResult> CopyPhoto (string id)
        {
            try
            {
                var oauth = this.HttpContext.Session.Get<AuthResponse> ("oauthResponse");
                if (oauth == null)
                    throw new OAuth2Exception ("Access token not found!");
                if (String.IsNullOrEmpty (id))
                    throw new ArgumentException ("Provided id is null or the empty string!");

                GoogleCredential credentials = GoogleCredential.FromAccessToken (oauth.AccessToken);

                using (var client = _httpFactory.CreateClient ())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue ("Bearer", oauth.AccessToken);
                    
                    var result = await client.GetAsync ($"{GOOGLE_PHOTOS_API_URL}/{id}");
                    var details = JsonConvert.DeserializeObject<MediaFileDetails> (await result.Content.ReadAsStringAsync ());

                    var bytes = await client.GetByteArrayAsync ($"{details.baseUrl}=d");
                    await System.IO.File.WriteAllBytesAsync (details.filename, bytes);
                    return Ok ("Image copyed");
                }
            }
            catch (Exception ex) { 
                return StatusCode (500, ex.Message);
            }
        }
        [HttpGet("GetFiles")]
        public async Task<ActionResult> GetListFiles()
        {
            try
            {
                var oauth = this.HttpContext.Session.Get<AuthResponse> ("oauthResponse");
                if (oauth == null)
                    throw new OAuth2Exception ("Access token not found!");

                GoogleCredential credentials = GoogleCredential.FromAccessToken (oauth.AccessToken);

                DriveService service = new DriveService (new BaseClientService.Initializer ()
                {
                    HttpClientInitializer = credentials,
                    ApplicationName = "Testing Drive",
                    ValidateParameters = false
                });
                
                var request = service.Files.List ();

                request.IncludeItemsFromAllDrives = true;
                request.PageSize = 200;
                request.AccessToken = oauth.AccessToken;
                request.SupportsAllDrives = true;
                request.Corpora = "allDrives";

                request.Fields = "files(id, name, mimeType, parents, createdTime, webViewLink, fileExtension, size)";
                var result = await request.ExecuteAsync ();

                return Ok (Json (result.Files.Select (el => new {Id = el.Id, Mime = el.MimeType, Name = el.Name, Label = el.LabelInfo })));
            }
            catch (Exception ex) {
                return StatusCode (500, ex.Message);
            }
        }

        [HttpGet ("GetPhotos")]
        public async Task<ActionResult> GetListPhotos ()
        {
            try
            {
                var oauth = this.HttpContext.Session.Get<AuthResponse> ("oauthResponse");
                if (oauth == null)
                    throw new OAuth2Exception ("Access token not found! Session has probably expired.");

                using (var client = _httpFactory.CreateClient ())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue ("Bearer", oauth.AccessToken);

                    var urlRequest = GOOGLE_PHOTOS_API_URL;
                    var response = await client.GetAsync ($"{urlRequest}");
                    List<MediaFileDetails> files = new List<MediaFileDetails> ();
                    Boolean hasNextPage = true;
                    DateTime timeStart = DateTime.Now;
                    do
                    {
                        // Deserializing the JSON response and checking for the presence of a nextPageToken which
                        // indicates the existence of more items to be requested
                        var content = JsonConvert.DeserializeObject<DetailsFiles> (await response.Content.ReadAsStringAsync ());
                        files.AddRange (content.mediaItems.ToList ());

                        if (!String.IsNullOrEmpty (content.nextPageToken))
                        {
                            // making a request with the nextPageToken as parameter to the GET request
                            response = await client.GetAsync ($"{urlRequest}?&pageToken={content.nextPageToken}");
                        }
                        else
                        {
                            hasNextPage = false;
                        }

                    } while (hasNextPage);
                    DateTime timeEnd = DateTime.Now;
                    files.Last ().filename += "   " + timeEnd.Subtract(timeStart).Seconds;

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
                return StatusCode (500, ex.Message + " Trace " + ex.StackTrace);
            }
        }
        [HttpGet ("GetPhotos2")]
        public async Task<ActionResult> GetListPhotos2 ()
        {
            try
            {
                var oauth = this.HttpContext.Session.Get<AuthResponse> ("oauthResponse");
                if (oauth == null)
                    throw new OAuth2Exception ("Access token not found! Session has probably expired.");

                using (var client = _httpFactory.CreateClient ())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue ("Bearer", oauth.AccessToken);

                    var urlRequest = GOOGLE_PHOTOS_API_URL;

                    List<MediaFileDetails> files = new List<MediaFileDetails> ();
                    Boolean hasNextPage = true;

                    var content = await MakeRequest<DetailsFiles> (urlRequest, client);
                    files.AddRange (content.mediaItems.ToList());

                    DateTime timeStart = DateTime.Now;
                    do
                    {
                        if (!String.IsNullOrEmpty (content.nextPageToken))
                        {
                            content = await MakeRequest<DetailsFiles> (urlRequest + $"?&pageToken={content.nextPageToken}", client);
                            files.AddRange (content.mediaItems.ToList ());
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
                return StatusCode (500, ex);
            }
        }
        // passing HttpClient object to avoid the overhead of getting a client from the factory when repeated requests are made due to pagination
        private async Task<T> MakeRequest<T> (string url, HttpClient client)
        {
            try
            {
                var response = await client.GetAsync (url);
                var content = JsonConvert.DeserializeObject<T> (await response.Content.ReadAsStringAsync ());
                return (content);
            }
            catch (Exception ex) 
            {
                throw new HttpRequestException(ex.Message, ex.InnerException) ;
            }
        }
        [HttpGet ("GetEmails")]
        public async Task<ActionResult> GetEmails (CancellationToken cancellationToken)
        {
            try
            {
                var oauth = this.HttpContext.Session.Get<AuthResponse> ("oauthResponse");
                if (oauth == null)
                    throw new OAuth2Exception ("Access token not found! Session has probably expired.");

                var credentials = GoogleCredential.FromAccessToken (oauth.AccessToken);
                var service = new GmailService (new BaseClientService.Initializer ()
                {
                    HttpClientInitializer = credentials,
                    ApplicationName = "Testing Drive",
                    ValidateParameters = false
                });

                var request = service.Users.Messages.List("ovi.lacatusu@gmail.com");
                request.LabelIds = "INBOX";
                request.IncludeSpamTrash = false;
                request.AccessToken = oauth.AccessToken;

                var result = await request.ExecuteAsync ();
                return Ok (result.Messages);
            }
            catch (Exception ex) {
                return StatusCode (500, ex);
            }
        }
    }
}