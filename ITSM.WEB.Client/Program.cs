using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services; // <--- 1. AGREGADO: Necesario para AddMudServices
using ITSM.WEB.Client.Auth;
using ITSM.WEB.Client.Servicios;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// --- CORRECCIÓN VITAL ---
// Registramos los servicios de MudBlazor (DialogService, Snackbar, etc.) en el Cliente.
builder.Services.AddMudServices();

// Servicios de Terceros (Manteniendo lo que tenías)
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();

// Servicios Propios (Manteniendo lo que tenías)
builder.Services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacion>();
builder.Services.AddScoped<ServicioSesion>();
builder.Services.AddScoped<UsuarioServicio>();
builder.Services.AddScoped<TicketServicio>();
builder.Services.AddScoped<InventarioServicio>();

// Configurar HttpClient (Manteniendo lo que tenías)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();