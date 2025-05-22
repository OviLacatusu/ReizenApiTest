using Blazored.SessionStorage;
using GoogleAccess.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Reizen.Data.Models.CQRS;
using Reizen.Data.Repositories;
using Reizen.Domain.Services;
using ReizenWebBlazor;
using ReizenWebBlazor.Client.Models;
using ReizenWebBlazor.Client.Services;
using ReizenWebBlazor.Components;
using ReizenWebBlazor.Models;
using System.Net;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder (args);

// Add services to the container.
builder.Services.AddRazorComponents ()
    .AddInteractiveServerComponents ();
    //.AddInteractiveWebAssemblyComponents ();

var clientID = builder.Configuration.GetSection ("OAuthConfig").GetValue<string> ("ClientID");
var clientSecret = builder.Configuration.GetSection ("OAuthConfig").GetValue<string> ("ClientSecret");

builder.Services.AddBlazoredSessionStorage ();

builder.Logging.ClearProviders ();
builder.Logging.AddConsole();

builder.Services.AddTransient<IServiceProvider, ServiceProvider> ();

// needed for AuthResponse
builder.Services.AddTransient<GoogleAuthService> ();


builder.Services.AddCors ();

builder.Services.AddHttpClient ("", client =>
{
    //client.BaseAddress = new Uri ("https://ovilacatusu-002-site1.qtempurl.com/");
    client.BaseAddress = new Uri ("https://localhost:7285/");
    
});
builder.Services.AddAuthentication (options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ExternalScheme;
}).AddCookie ()
 .AddGoogle (o => {
    o.ClientId = clientID;
    o.ClientSecret = clientSecret;
    o.SaveTokens = true;
    });

builder.Services.AddControllers ()
    .AddJsonOptions (options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build ();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment ())
{
    app.UseWebAssemblyDebugging ();
}
else
{
    app.UseExceptionHandler ("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts ();
}
app.MapControllers ();

app.UseCors ();

app.UseHttpsRedirection ();

app.UseAuthorization ();
app.UseAuthorization ();

app.UseStaticFiles ();

app.UseAntiforgery ();

app.MapStaticAssets ();

app.MapRazorComponents<App> ()
    .AddInteractiveServerRenderMode ()
    //.AddInteractiveWebAssemblyRenderMode ()
    .AddAdditionalAssemblies (typeof (ReizenWebBlazor.Client._Imports).Assembly);

app.Run ();
