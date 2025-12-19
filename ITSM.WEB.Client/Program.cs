using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using ITSM.WEB.Client.Servicios;
using ITSM.WEB.Client.Auth;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// 1. Cliente HTTP
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// 2. MudBlazor
builder.Services.AddMudServices();

// 3. SEGURIDAD Y SESIÓN (NUEVO)
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<ServicioSesion>();
builder.Services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacion>();

builder.Services.AddScoped<TicketServicio>();

await builder.Build().RunAsync();