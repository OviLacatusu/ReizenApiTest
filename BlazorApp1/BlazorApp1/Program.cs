using BlazorApp1.Components;
using BlazorApp1.Components.Account;
using BlazorApp1.Data;
using BlazorApp1.Models;
using Blazored.SessionStorage;
using GoogleAccess.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Reizen.CommonClasses;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder (args);

// Add services to the container.
builder.Services.AddRazorComponents ()
    .AddInteractiveServerComponents ()
    //.AddInteractiveWebAssemblyComponents ()
    .AddAuthenticationStateSerialization ();

var clientID = builder.Configuration.GetSection("Settings").GetSection ("OAuthSettings").GetValue<string> ("ClientID");
var clientSecret = builder.Configuration.GetSection("Settings").GetSection ("OAuthSettings").GetValue<string> ("ClientSecret");
var scopes = builder.Configuration.GetSection("Settings").GetSection ("OAuthSettings").GetSection ("OAuthScopes").Get<string[]>();

var connectionString = builder.Configuration.GetConnectionString ("ReizenDB2") ?? throw new InvalidOperationException ("Connection string not found.");

builder.Services.AddDbContext<ApplicationDbContext> (options =>
    options.UseSqlServer (connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter ();

var test = builder.Configuration.GetSection ("Settings");
builder.Services.Configure<ConfigOptions> (test);

builder.Services.AddBlazoredSessionStorage ();
builder.Services.AddMemoryCache ();
builder.Services.AddScoped<IMemoryCache, MemoryCache> ();

builder.Logging.ClearProviders ();
builder.Logging.AddConsole ();

builder.Services.AddCors ();

builder.Services.AddTransient<CustomAuthDelegatingHandler> ();
builder.Services.AddHttpClient ("", client => 
{ 
    client.BaseAddress = new Uri (ConfigOptions.httpReizenApiUri); 
});
builder.Services.AddHttpClient ("GoogleAccess", client =>
{
    client.BaseAddress = new Uri (ConfigOptions.httpReizenApiUri);

}).AddHttpMessageHandler<CustomAuthDelegatingHandler> ();

builder.Services.AddControllers ()
    .AddJsonOptions (options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddCascadingAuthenticationState ();
builder.Services.AddScoped<SignInManager<ApplicationUser>> ();
builder.Services.AddScoped<IdentityUserAccessor> ();
builder.Services.AddScoped<IdentityRedirectManager> ();

builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider> ();

builder.Services.AddIdentity<ApplicationUser, IdentityRole> (options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext> ()
.AddSignInManager ()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication (options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    
    //options.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;

}).AddCookie ()
.AddGoogle (options =>
{
    options.ClientId = clientID;
    options.ClientSecret = clientSecret;
    options.SaveTokens = true;
    options.ClaimActions.MapAll ();
    // Adding scopes; could not find another way but to add them one by one
    scopes?.ToList ().ForEach (el => options.Scope.Add (el));
    
    // Saving OAuth 2.0 tokens
    options.Events.OnTicketReceived = ctx =>
    {
        List<AuthenticationToken>? tokens = ctx.Properties?.GetTokens ().ToList ();        
        ctx?.Properties?.StoreTokens (tokens);
        
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

app.UseCors ();

app.UseHttpsRedirection ();

app.UseAuthentication ();
app.UseAuthorization ();

app.UseStaticFiles ();
app.UseAntiforgery ();

app.MapStaticAssets ();
app.MapRazorComponents<App> ()
    .AddInteractiveServerRenderMode ();
    //.AddInteractiveWebAssemblyRenderMode ();
    //.AddAdditionalAssemblies (typeof (BlazorApp1.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints ();

app.MapPost ("/account/logout", async (HttpContext context) => 
{   
    await context.SignOutAsync (IdentityConstants.ApplicationScheme);
    context.Response.Redirect ("/");

}).RequireAuthorization();
app.Run ();
