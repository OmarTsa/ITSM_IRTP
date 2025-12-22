using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization; // NECESARIO
using MudBlazor.Services;
using ITSM.WEB.Client.Servicios;
using ITSM.WEB.Client.Auth; // NECESARIO para ProveedorAutenticacion

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();
builder.Services.AddAuthorizationCore(); // 1. Habilita la seguridad en Blazor

// Configurar HttpClient
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// 2. Registrar Servicios de Sesión y Autenticación
builder.Services.AddScoped<ServicioSesion>();
builder.Services.AddScoped<ProveedorAutenticacion>();

// 3. Conectar el Proveedor de Autenticación con Blazor
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<ProveedorAutenticacion>());

// Servicios de Negocio (Cliente)
builder.Services.AddScoped<TicketServicio>();

await builder.Build().RunAsync();