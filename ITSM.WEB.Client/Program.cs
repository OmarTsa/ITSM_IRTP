using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using ITSM.WEB.Client.Auth;
using ITSM.WEB.Client.Servicios;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// REGISTRO DE SERVICIOS
builder.Services.AddScoped<TicketServicio>();
builder.Services.AddScoped<ServicioSesion>();
builder.Services.AddScoped<UsuarioServicio>(); // <--- ¡ESTA LÍNEA ES VITAL!

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacion>();

builder.Services.AddMudServices();

await builder.Build().RunAsync();