using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Http;

namespace GoogleAccess.Domain.Models
{
    public static class SessionExtension
    {
        private static readonly JsonSerializerSettings _options = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Error, StringEscapeHandling = StringEscapeHandling.Default };

        public static void Set<T> (this ISession session, string key, T value)
        {
            session.SetString (key, JsonConvert.SerializeObject (value, _options));

        }
        public static T? Get<T> (this Microsoft.AspNetCore.Http.ISession session, string key)
        {
            var value = session.GetString (key);
            return value == null ? default : JsonConvert.DeserializeObject<T> (value, _options);
        }
    }
}
