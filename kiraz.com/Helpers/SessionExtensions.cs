using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace kiraz.com.Helpers
{
    public static class SessionExtensions
    {
        public static void SetObj<T>(this ISession session, string key, T value) =>
            session.SetString(key, JsonSerializer.Serialize(value));

        public static T? GetObj<T>(this ISession session, string key)
        {
            var json = session.GetString(key);
            return string.IsNullOrEmpty(json) ? default : JsonSerializer.Deserialize<T>(json);
        }
    }
}
