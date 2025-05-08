using Google.Apis.Admin.Directory.directory_v1.Data;
using Google.Apis.Sheets.v4;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using System.Threading;
using Google.Apis.Services;
using System.Net;
using System.Reflection;

namespace WebApplication2.Models
{
    public class OAuth2Helper 
    {
        //private const string CREDENTIALS_FOLDER = ".credentials";
        //private readonly string _clientSecretPath;
        private readonly string _applicationName;
        private readonly GoogleAuthConfig _config;

        public OAuth2Helper (string applicationName, GoogleAuthConfig config)       
        {
            //_clientSecretPath = clientSecretPath ?? throw new ArgumentNullException (nameof (clientSecretPath));
            this._applicationName = applicationName ?? throw new ArgumentNullException (nameof (applicationName));
            this._config = config ?? throw new ArgumentNullException (nameof(config));

            //if (!File.Exists (_clientSecretPath))
            //{
            //    throw new FileNotFoundException ($"Client secret file not found at: {_clientSecretPath}");
            //}
        }

        public T GetService<T>() where T : BaseClientService
        {
            try
            {
                var credential = GetUserCredentials();
                return CreateService<T>(credential);
            }
            catch (Exception ex)
            {
                throw new OAuth2Exception($"Failed to create service of type {typeof(T).Name}", ex);
            }
        }

        public GoogleCredential GetUserCredentials()
        {
            try
            {
                //if (string.IsNullOrEmpty(username))
                //    throw new ArgumentNullException(nameof(username));

                //using (var stream = new FileStream(_clientSecretPath, FileMode.Open, FileAccess.Read))
                //{
                //    string credPath = Path.Combine(
                //        Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                //        CREDENTIALS_FOLDER,
                //        Assembly.GetExecutingAssembly().GetName().Name);

                //var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                //    GoogleClientSecrets.FromStream(stream).Secrets,
                //    scopes,
                //    username,
                //    CancellationToken.None,
                //    new FileDataStore(credPath, true)).Result;
                var credential = GoogleCredential.FromAccessToken (_config.AccessToken);
                //UserCredential cred = new UserCredential(username, credential);
                
                    
                    //credential.GetAccessTokenForRequestAsync();
                return credential;
                //}
            }
            catch (Exception ex)
            {
                throw new OAuth2Exception("Failed to get user credentials", ex);
            }
        }

        private T CreateService<T>(GoogleCredential credential) where T : BaseClientService
        {
            try
            {
                if (credential == null)
                    throw new ArgumentNullException(nameof(credential));

                var initializer = new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = _applicationName
                };

                return (T)Activator.CreateInstance(typeof(T), initializer);
            }
            catch (Exception ex)
            {
                throw new OAuth2Exception($"Failed to create service of type {typeof(T).Name}", ex);
            }
        }
    }
    public class OAuth2Exception : Exception
    {
        public OAuth2Exception(string message) : base(message) { }
        public OAuth2Exception(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}