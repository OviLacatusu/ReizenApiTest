

using Blazored.SessionStorage;
using GoogleAccess.Domain.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Hosting;
using ReizenWebBlazor.Client.Models;
using ReizenWebBlazor.Client.Services;
using System.Net.Http.Json;

var builder = WebAssemblyHostBuilder.CreateDefault (args);
// making a request to the minimal API on ReizenApi to get the configuration settings for Google OAuth
HttpClient clientH = new HttpClient ();

//clientH.BaseAddress = new Uri ("https://ovilacatusu-002-site1.qtempurl.com/");
clientH.BaseAddress = new Uri ("https://localhost:7285/");
var config = await clientH.GetFromJsonAsync<GoogleAuthConfig> ("api/GetServerOauthConfig");
//builder.Services.AddHostedService<PollingAPIService> ();
builder.Services.AddTransient<GoogleAuthConfig> (x => config);
builder.Services.AddTransient<GoogleAuthService> ();
builder.Services.AddBlazoredSessionStorage ();

//builder.Services.AddHttpClient ("", client =>
//{
//    //client.BaseAddress = new Uri ("https://ovilacatusu-002-site1.qtempurl.com/");
//    client.BaseAddress = new Uri ("https://localhost:7217/");
//});
await builder.Build ().RunAsync ();
