using ITSM.Datos;
using ITSM.Negocio;
using ITSM.WEB.Components;
using Microsoft.EntityFrameworkCore;
using Blazored.LocalStorage;
using ITSM.WEB.Client.Servicios;
using ITSM.WEB.Client.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services; // <--- ESTA LÍNEA ES VITAL PARA CORREGIR EL ERROR CS1061

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Ahora sí reconocerá este método gracias al using de arriba
builder.Services.AddMudServices();

// --- 1. CONFIGURACIÓN DE BASE DE DATOS (Oracle) ---
builder.Services.AddDbContext<ContextoBD>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- 2. SERVICIOS DE NEGOCIO (Lógica del Servidor) ---
builder.Services.AddScoped<UsuarioNegocio>();
builder.Services.AddScoped<TicketNegocio>();
builder.Services.AddScoped<ActivoNegocio>();

// --- 3. SERVICIOS CLIENTE (Espejo para Pre-rendering) ---
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacion>();
builder.Services.AddScoped<ServicioSesion>();
builder.Services.AddScoped<UsuarioServicio>();
builder.Services.AddScoped<TicketServicio>();
builder.Services.AddScoped<InventarioServicio>();

// Configurar HttpClient para el servidor (apuntando a sí mismo)
// Nota: Ajusta el puerto si es necesario según tu launchSettings.json
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7233")
});

// Soporte para Controladores API
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
app.UseStaticFiles();
app.UseAntiforgery();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ITSM.WEB.Client._Imports).Assembly);

app.Run();