using MudBlazor.Services;
using Microsoft.EntityFrameworkCore;
using ITSM.Datos;
using ITSM.Negocio;
using ITSM.WEB.Components;
using Microsoft.AspNetCore.Components;
using ITSM.WEB.Client.Servicios;
using ITSM.WEB.Client.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies; // <--- 1. IMPORTANTE: Agregar este namespace

var builder = WebApplication.CreateBuilder(args);

// --- BASE DE DATOS ---
var connectionString = builder.Configuration.GetConnectionString("ConexionOracle");
builder.Services.AddDbContext<ContextoBD>(options => options.UseOracle(connectionString));

// --- SERVICIOS DE NEGOCIO ---
builder.Services.AddScoped<UsuarioNegocio>();
builder.Services.AddScoped<TicketNegocio>();

// --- SERVICIOS WEB ---
builder.Services.AddControllers();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();

// --- HTTP CLIENT ---
builder.Services.AddScoped(sp =>
{
    var navigation = sp.GetRequiredService<NavigationManager>();
    return new HttpClient { BaseAddress = new Uri(navigation.BaseUri) };
});

// --- SERVICIOS CLIENTE ---
builder.Services.AddScoped<TicketServicio>();

// --- SEGURIDAD Y SESIÓN (AQUÍ ESTÁ LA SOLUCIÓN) ---
builder.Services.AddScoped<ServicioSesion>();
builder.Services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacion>();

// 2. AGREGAR EL SERVICIO DE AUTENTICACIÓN
// Esto evita el error "No authenticationScheme was specified"
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login"; // A dónde redirigir si no tiene permiso
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

// Nota: AddAuthorizationCore es para WASM, en Server usamos AddAuthorization normal o el default
builder.Services.AddAuthorization();

var app = builder.Build();

// --- PIPELINE ---
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

// 3. ACTIVAR LOS MIDDLEWARES (EN ESTE ORDEN EXACTO)
// Deben ir DESPUÉS de UseStaticFiles y ANTES de MapControllers
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ITSM.WEB.Client._Imports).Assembly);

app.Run();