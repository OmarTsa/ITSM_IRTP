using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using ITSM.WEB.Client.Servicios;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();

// Configurar HttpClient para que apunte a la API del Servidor
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Registrar Servicios del Cliente (Que usan la API)
builder.Services.AddScoped<TicketServicio>();

await builder.Build().RunAsync();