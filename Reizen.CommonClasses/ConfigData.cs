using static System.Net.WebRequestMethods;

namespace Reizen.CommonClasses
{
    public sealed class ConfigOptions
    {
        public static readonly string httpReizenApiUri="https://ovilacatusu-002-site2.qtempurl.com/";
        //public static readonly string httpReizenApiUri = "https://localhost:7285/";

        public string ApiUrl { get; set; }
        public string ExternalAuthMethod { get; set; }
        public OAuthConfig OAuthSettings { get; set; }

        public class OAuthConfig
        {
            public string ClientSecret { get; set; }
            public string ClientId { get; set; }
            public string[] OAuthScopes { get; set; }
        }
    }
}
