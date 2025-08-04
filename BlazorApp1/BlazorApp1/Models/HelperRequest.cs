using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Reizen.CommonClasses;

namespace BlazorApp1.Models
{
    public class HelperRequest
    {
        public static async Task<Result<T?>> SendGetRequestAndParseJsonAsync<T> (string uri, HttpClient httpClient) 
        {
            try
            {
                var result = await httpClient.GetAsync (uri);
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadFromJsonAsync<T> ();
                    return Result<T?>.Success (content);
                }
                else
                {
                    var error = await result.Content.ReadAsStringAsync ();
                    var statusCode = result.StatusCode;

                    return Result<T?>.Failure ($"{statusCode}: {error}");
                }
            }
            catch (Exception ex) 
            {
                return Result<T?>.Failure ("Something went wrong");
            }
        }
        // Not tested
        public static async Task<Result<T?>> SendPostRequestAndParseJsonAsync<T> (string uri, HttpClient httpClient, T objToSerialize)
        {
            try
            {
                var httpContent = JsonSerializer.Serialize<T> (objToSerialize);
                var postContent = new StringContent (httpContent, Encoding.UTF8, "application/json");
                var result = await httpClient.PostAsync (uri, postContent);
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadFromJsonAsync<T> ();
                    return Result<T?>.Success (content);
                }
                else
                {
                    var error = await result.Content.ReadAsStringAsync ();
                    var statusCode = result.StatusCode;

                    return Result<T?>.Failure ($"{statusCode}: {error}");
                }
            }
            catch (Exception ex)
            {
                return Result<T?>.Failure ("Something went wrong");
            }
        }

    }
}

