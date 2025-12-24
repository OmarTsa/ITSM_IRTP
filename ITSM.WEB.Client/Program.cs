using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using ITSM.WEB.Client.Auth;
using ITSM.WEB.Client.Servicios;

var builder = WebAssemblyHostBuilder.CreateDefault(args); // <--- ESTA LÍNEA ES VITAL

// Servicios de Terceros
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();

// Servicios Propios
builder.Services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacion>();
builder.Services.AddScoped<ServicioSesion>();
builder.Services.AddScoped<UsuarioServicio>();
builder.Services.AddScoped<TicketServicio>();
builder.Services.AddScoped<InventarioServicio>(); // Si tienes este servicio

// Configurar HttpClient
// Esto asegura que el cliente pueda llamar a la API del servidor (mismo origen)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();