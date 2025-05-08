
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace GoogleAccess.Domain.Models
{


    public static class SessionExtension
    {
        private static readonly JsonSerializerOptions _options = new () { PropertyNameCaseInsensitive = true, UnmappedMemberHandling = System.Text.Json.Serialization.JsonUnmappedMemberHandling.Skip  };

        public static void Set<T> (this ISession session, string key, T value)
        {
            session.Set<string>(key, JsonSerializer.Serialize (value));
        }

        public static T? Get<T> (this ISession session, string key)
        {
            var value = session.Get<string> (key);
            return value == null ? default : JsonSerializer.Deserialize<T> (value);
        }
    }
}
