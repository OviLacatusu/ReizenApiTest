using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Reizen.Data.Models.CQRS;
using Reizen.Domain.Services;

var builder = WebAssemblyHostBuilder.CreateDefault (args);

builder.Services.AddHttpClient("",client => {
    client.BaseAddress = new Uri ("https://localhost:7285");
});
//builder.Services.AddTransient<IMediator> (_ => Mediator.MediatorFactory ());
//builder.Services.AddTransient<KlantenService> ();
//builder.Services.AddTransient<LandenService> ();

await builder.Build ().RunAsync ();
