using System.Net.Http.Json;
using Reizen.CommonClasses;

namespace BlazorApp1.Client
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
                    var error = await result?.Content.ReadAsStringAsync ();
                    var statusCode = result?.StatusCode;

                    return Result<T?>.Failure ($"{statusCode}: {error}");
                }
            }
            catch (Exception ex) 
            {
                return Result<T?>.Failure ("Something went wrong");
            }
        }
        // Not tested
        public static async Task<Result<T?>> SendPostRequestAndParseJsonAsync<T> (string uri, HttpClient httpClient, string httpContent)
        {
            try
            {
                var postContent = new StringContent (httpContent);
                var result = await httpClient.PostAsync (uri, postContent);
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadFromJsonAsync<T> ();
                    return Result<T?>.Success (content);
                }
                else
                {
                    var error = await result?.Content.ReadAsStringAsync ();
                    var statusCode = result?.StatusCode;

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

