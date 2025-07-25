using Blazored.SessionStorage;
using GoogleAccess.Domain.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http.Json;
using Reizen.CommonClasses;

var builder = WebAssemblyHostBuilder.CreateDefault (args);
builder.Services.AddHttpClient ("", client =>
{
    //client.BaseAddress = new Uri ("https://ovilacatusu-002-site1.qtempurl.com/");
    client.BaseAddress = new Uri (ConfigOptions.httpReizenApiUri);
});

builder.Services.AddBlazoredSessionStorage ();

builder.Services.AddAuthorizationCore ();

builder.Services.AddCascadingAuthenticationState ();
builder.Services.AddAuthenticationStateDeserialization ();

await builder.Build ().RunAsync ();
