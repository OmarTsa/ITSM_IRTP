using ITSM.Datos;
using ITSM.Negocio;
using ITSM.WEB.Components;
using Microsoft.EntityFrameworkCore;
using Blazored.LocalStorage;
using ITSM.WEB.Client.Servicios;
using ITSM.WEB.Client.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Servicios de Blazor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();

// Base de Datos
builder.Services.AddDbContext<ContextoBD>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

// Servicios de Negocio
builder.Services.AddScoped<UsuarioNegocio>();
builder.Services.AddScoped<TicketNegocio>();
builder.Services.AddScoped<ActivoNegocio>();

// Servicios Cliente
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacion>();
builder.Services.AddScoped<ServicioSesion>();
builder.Services.AddScoped<UsuarioServicio>();
builder.Services.AddScoped<TicketServicio>();
builder.Services.AddScoped<InventarioServicio>();

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7233")
});

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

// --- CORRECCIÓN CRÍTICA PARA .NET 10 ---
// Esta es la línea que soluciona el error 404 del blazor.web.js
app.MapStaticAssets();

// Mantenemos UseStaticFiles para tus archivos CSS/imágenes normales
app.UseStaticFiles();
app.UseAntiforgery();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ITSM.WEB.Client._Imports).Assembly);

app.Run();