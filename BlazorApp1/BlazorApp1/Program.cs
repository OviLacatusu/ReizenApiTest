using BlazorApp1.Client.Pages;
using BlazorApp1.Components;
using BlazorApp1.Components.Account;
using BlazorApp1.Data;
using Blazored.SessionStorage;
using GoogleAccess.Domain.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Google;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder (args);

// Add services to the container.
builder.Services.AddRazorComponents ()
    .AddInteractiveServerComponents ()
    .AddInteractiveWebAssemblyComponents ()
    .AddAuthenticationStateSerialization ();

var clientID = builder.Configuration.GetSection ("OAuthConfig").GetValue<string> ("ClientID");
var clientSecret = builder.Configuration.GetSection ("OAuthConfig").GetValue<string> ("ClientSecret");
var scopes = builder.Configuration.GetSection ("OAuthConfig").GetSection ("OAuthScopes").Get<string[]>();

builder.Services.AddBlazoredSessionStorage ();

builder.Logging.ClearProviders ();
builder.Logging.AddConsole ();

builder.Services.AddTransient<IServiceProvider, ServiceProvider> ();

// needed for AuthResponse
builder.Services.AddTransient<GoogleAuthService> ();

builder.Services.AddCors ();

builder.Services.AddHttpClient ("", client =>
{
    //client.BaseAddress = new Uri ("https://ovilacatusu-002-site1.qtempurl.com/");
    client.BaseAddress = new Uri ("https://localhost:7285/");
});

builder.Services.AddControllers ()
    .AddJsonOptions (options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddCascadingAuthenticationState ();
builder.Services.AddScoped<IdentityUserAccessor> ();
builder.Services.AddScoped<IdentityRedirectManager> ();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider> ();


var connectionString = builder.Configuration.GetConnectionString ("ReizenDB2") ?? throw new InvalidOperationException ("Connection string not found.");
builder.Services.AddDbContext<ApplicationDbContext> (options =>
    options.UseSqlServer (connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter ();

builder.Services.AddIdentityCore<ApplicationUser> (options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext> ()
    .AddSignInManager ()
    .AddDefaultTokenProviders ();

builder.Services.AddAuthentication (options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;

}).AddCookie ()
.AddGoogleOpenIdConnect (options =>
{
    options.ForwardSignIn = "https://accounts.google.com/gsi/client";
    options.ClientId = clientID;
    options.ClientSecret = clientSecret;
    // Adding scopes; could not find another way but to add them one by one
    scopes?.ToList ().ForEach (el => options.Scope.Add (el));
    options.SaveTokens = true;

    // Saving OAuth 2.0 tokens
    options.Events.OnTicketReceived = ctx =>
    {
        List<AuthenticationToken>? tokens = ctx.Properties?.GetTokens ().ToList ();
        var accessToken = tokens?.FirstOrDefault (t => t.Name == "access_token")?.Value;

        ctx.Properties?.StoreTokens (tokens);
        return Task.CompletedTask;
    };
});

builder.Services.AddHttpContextAccessor ();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender> ();

var app = builder.Build ();

// Configure the HTTP request pipeline. 
if (app.Environment.IsDevelopment ())
{
    app.UseWebAssemblyDebugging ();
    app.UseMigrationsEndPoint ();
}
else
{
    app.UseExceptionHandler ("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts ();
}
app.MapControllers ();

app.UseCors (builder => builder.AllowAnyHeader());

app.UseHttpsRedirection ();

app.UseAuthentication ();
app.UseAuthorization ();

app.UseStaticFiles ();

app.UseAntiforgery ();

app.MapStaticAssets ();
app.MapRazorComponents<App> ()
    .AddInteractiveServerRenderMode ()
    .AddInteractiveWebAssemblyRenderMode ()
    .AddAdditionalAssemblies (typeof (BlazorApp1.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints ();

app.Run ();
