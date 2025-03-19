using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using Reizen.Domain.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ReizenContext> (options => options.UseSqlServer( builder.Configuration.GetConnectionString("ReizenDB")));
builder.Services.AddScoped<IKlantenRepository, SQLKlantenRepository>();
builder.Services.AddScoped<KlantenService> ();
// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
