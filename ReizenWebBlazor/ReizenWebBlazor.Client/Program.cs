using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Reizen.Data.Models.CQRS;
using Reizen.Domain.Services;

var builder = WebAssemblyHostBuilder.CreateDefault (args);

builder.Services.AddTransient<IMediator> (_ => Mediator.MediatorFactory ());
builder.Services.AddTransient<KlantenService> ();

await builder.Build ().RunAsync ();
