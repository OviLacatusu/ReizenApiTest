using GoogleAccess.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Reizen.Data.Models.CQRS;
using Reizen.Data.Repositories;
using Reizen.Domain.Services;
using ReizenWebBlazor;
using ReizenWebBlazor.Components;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder (args);

//var clientID = builder.Configuration.GetSection ("OAuthConfig").GetValue<string> ("ClientID");
// Add services to the container.
builder.Services.AddRazorComponents ()
    .AddInteractiveServerComponents ()
    .AddInteractiveWebAssemblyComponents ();

var clientID = builder.Configuration.GetSection ("OAuthConfig").GetValue<string> ("ClientID");

builder.Services.AddDbContextFactory<ReizenContext> (options => options.UseSqlServer (builder.Configuration.GetConnectionString ("ReizenDB2")));

builder.Services.AddTransient<IMediator> (_ => Mediator.MediatorFactory ());
builder.Services.AddScoped<IKlantenRepository, KlantenService> ();
builder.Services.AddScoped<ILandenWerelddelenRepository, LandenService> ();
builder.Services.AddScoped<IReizenRepository, ReizenService> ();

GoogleAuthConfig config = new GoogleAuthConfig
{
    ClientId = clientID,
    ClientSecret = "",
    AuthAccessType = "offline",
    AuthRedirectUrl = "https://ovilacatusu-002-site1.qtempurl.com/Test/HandleCallback",
    AuthScope = new string[] { "https://www.googleapis.com/auth/drive.file", "https://www.googleapis.com/auth/drive", "https://www.googleapis.com/auth/drive.readonly", "https://www.googleapis.com/auth/photoslibrary.readonly", "https://www.googleapis.com/auth/gmail.readonly", "https://mail.google.com/" },
    SpreadsheetId = "1zfw5SOA99VtpGcsIzgiY5h5J3lJDLsLtJy2NYBEdl7k",
    ClientSecretPath = string.Concat (AppContext.BaseDirectory.ToString (), $"client_secret_{clientID}.json")
};
builder.Services.AddTransient<GoogleAuthConfig> (x => config);
builder.Services.AddTransient<GoogleAuthService> ();

builder.Services.AddHttpClient ();

builder.Services.AddMvcCore ();

builder.Services.AddCors ();
builder.Services.AddAutoMapper (typeof (AutoMapperProfile));

// Add services to the container.

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
app.MapGet ("/api/GetServerOauthConfig",

    () => Results.Ok (config));

app.MapControllers ();

app.UseCors ();

app.UseHttpsRedirection ();

app.UseAntiforgery ();

app.MapStaticAssets ();

app.MapRazorComponents<App> ()
    .AddInteractiveServerRenderMode ()
    .AddInteractiveWebAssemblyRenderMode ()
    .AddAdditionalAssemblies (typeof (ReizenWebBlazor.Client._Imports).Assembly);

app.Run ();
