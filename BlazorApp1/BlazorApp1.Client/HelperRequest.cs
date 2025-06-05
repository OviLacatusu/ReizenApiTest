using System.Net.Http.Json;

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
                    return Result<T>.Success (content);
                }
                else
                {
                    var message = result?.RequestMessage;
                    var statusCode = result?.StatusCode;

                    return Result<T>.Failure ($"{statusCode}:  {message}");
                }
            }
            catch (Exception ex) 
            {
                return Result<T>.Failure ("Something went wrong");
            }
        }

        // Code duplication for testing pourposes
        public class Result<T>
        {
            public bool IsSuccessful
            {
                get;
            }
            public T? Value
            {
                get;
            }
            public string? Error
            {
                get;
            }
            private Result (bool isSuccess, T? value, string? error = null)
            {
                IsSuccessful = isSuccess;
                Value = value;
                Error = error;
            }
            public static Result<T> Success (T value) => new (true, value);
            public static Result<T> Failure (string error) => new (false, default, error);
        }
    }
}

