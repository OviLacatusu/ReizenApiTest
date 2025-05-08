
using Google.Apis.Auth.OAuth2;
using GoogleAccess.Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Reizen.Data.Models.CQRS;
using Reizen.Data.Repositories;
using Reizen.Domain.Services;
using ReizenApi;
using ReizenWebBlazor.Client.Models;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var clientID = builder.Configuration.GetSection ("OAuthConfig").GetValue<string> ("ClientID");

builder.Services.AddDbContextFactory<ReizenContext> (options => options.UseSqlServer( builder.Configuration.GetConnectionString("ReizenDB2")));

builder.Services.AddTransient<IMediator> (_ => Mediator.MediatorFactory ());
builder.Services.AddScoped<IKlantenRepository,KlantenService> ();
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

//builder.Services.AddSingleton<OAuthServerSettings> (x =>
//{
//    var httpClient = x.GetRequiredService<HttpClient> ();
//    var result = httpClient.GetFromJsonAsync<OAuthServerSettings> (httpClient.BaseAddress + "Test/GetOauthServerSettings").ConfigureAwait (false).GetAwaiter ().GetResult ();
//    return result;
//}
//    );

builder.Services.AddHttpClient ();

builder.Services.AddMvcCore ();

builder.Services.AddCors ();
builder.Services.AddAutoMapper (typeof (AutoMapperProfile));

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions( options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles );

var app = builder.Build();

// minimal api for sending the Google OAuth config
app.MapGet ("/api/GetServerOauthConfig",

    () => Results.Ok(config));

// Configure the HTTP request pipeline.


app.UseCors (builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()); 

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
