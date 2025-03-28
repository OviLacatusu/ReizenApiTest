using Microsoft.EntityFrameworkCore;
using Reizen.Data.Models.CQRS;
using Reizen.Data.Repositories;
using Reizen.Domain.Services;
using ReizenWebBlazor.Components;

var builder = WebApplication.CreateBuilder (args);
// Add services to the container.
builder.Services.AddRazorComponents ()
    //.AddInteractiveServerComponents ()
    .AddInteractiveWebAssemblyComponents ();

builder.Services.AddDbContext<ReizenContext> (options => options.UseSqlServer (builder.Configuration.GetConnectionString ("ReizenDB")));
builder.Services.AddTransient<IMediator> (_ => Mediator.MediatorFactory ());
builder.Services.AddTransient<KlantenService> ();


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

app.UseHttpsRedirection ();


app.UseAntiforgery ();

app.MapStaticAssets ();
app.MapRazorComponents<App> ()
    //.AddInteractiveServerRenderMode ()
    .AddInteractiveWebAssemblyRenderMode ()
    .AddAdditionalAssemblies (typeof (ReizenWebBlazor.Client._Imports).Assembly);

app.Run ();
