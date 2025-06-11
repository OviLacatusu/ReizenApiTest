
using Microsoft.EntityFrameworkCore;
using Reizen.Data.Models.CQRS;
using Reizen.Data.Repositories;
using Reizen.Data.Services;
using ReizenApi;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var clientID = builder.Configuration.GetSection ("OAuthConfig").GetValue<string> ("ClientID");
var clientSecret = builder.Configuration.GetSection ("OAuthConfig").GetValue<string> ("ClientSecret");

builder.Services.AddDbContextFactory<ReizenContext> (options => options.UseSqlServer( builder.Configuration.GetConnectionString("ReizenDB2")));

builder.Services.AddTransient<IMediator> (_ => Mediator.MediatorFactory ());

builder.Services.AddScoped<IKlantenRepository,KlantenService> ();
builder.Services.AddScoped<ILandenWerelddelenRepository, LandenService> ();
builder.Services.AddScoped<IReizenRepository, ReizenService> ();
builder.Services.AddScoped<IBoekingenRepository, BoekingenService> ();

builder.Services.AddHttpContextAccessor ();

builder.Logging.ClearProviders ();
builder.Logging.AddConsole ();

builder.Services.AddDistributedMemoryCache ();
builder.Services.AddSession ();

builder.Services.AddHttpClient ();

builder.Services.AddCors ();
builder.Services.AddAutoMapper (typeof (AutoMapperProfile));

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions( options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles );

var app = builder.Build();

app.UseCors (builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseSession ();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
