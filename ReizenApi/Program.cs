
using Microsoft.EntityFrameworkCore;
using Reizen.Data.Models.CQRS;
using Reizen.Data.Repositories;
using Reizen.Domain.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContextFactory<ReizenContext> (options => options.UseSqlServer( builder.Configuration.GetConnectionString("ReizenDB")));
builder.Services.AddTransient<IMediator> (_ => Mediator.MediatorFactory ());
builder.Services.AddScoped<IKlantenRepository,KlantenService> ();
builder.Services.AddScoped<ILandenWerelddelenRepository, LandenService> ();
builder.Services.AddScoped<IReizenRepository, ReizenService> ();
builder.Services.AddCors ();

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions( options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles );

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors (builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()); 

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
