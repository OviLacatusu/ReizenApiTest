using GoogleAccess.Domain.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace ReizenWebBlazor.Models
{
    public class SessionStorage : IBrowserStorage
    {
        private ProtectedSessionStorage Storage {get; set;}
        public SessionStorage (ProtectedSessionStorage storage)
        {
            Storage = storage;
        }
        
        public async Task DeleteAsync (string key)
        {
            await Storage.DeleteAsync (key);
        }
        public async Task<T?> GetAsync<T> (string key)
        {
            var value = await Storage.GetAsync<T> (key);
            if (value.Success)
            {
                return value.Value;
            }
            else
            {
                return default (T);
            }
        }
        public async Task SetAsync (string key, object value)
        {
            await Storage.SetAsync (key, value);
        }
    }
}
