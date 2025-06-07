using Blazored.SessionStorage;
using GoogleAccess.Domain.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http.Json;
using Reizen.CommonClasses;

var builder = WebAssemblyHostBuilder.CreateDefault (args);
HttpClient clientH = new HttpClient ();

//clientH.BaseAddress = new Uri ("https://ovilacatusu-002-site1.qtempurl.com/");
clientH.BaseAddress = new Uri (ConfigData.httpClientURI);
//var config = await clientH.GetFromJsonAsync<GoogleAuthConfig> ("api/GetServerOauthConfig");

//builder.Services.AddTransient<GoogleAuthConfig> (x => config);
//builder.Services.AddTransient<GoogleAuthService> ();

builder.Services.AddBlazoredSessionStorage ();

builder.Services.AddAuthorizationCore ();

builder.Services.AddCascadingAuthenticationState ();
builder.Services.AddAuthenticationStateDeserialization ();

await builder.Build ().RunAsync ();
