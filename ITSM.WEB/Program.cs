using MudBlazor.Services;
using Microsoft.EntityFrameworkCore;
using ITSM.Datos;
using ITSM.Negocio;
using ITSM.WEB.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using ITSM.WEB.Client.Auth;
using ITSM.WEB.Client.Servicios;

var builder = WebApplication.CreateBuilder(args);

// 1. CONEXIÓN A BASE DE DATOS ORACLE IRTP
var connectionString = builder.Configuration.GetConnectionString("ConexionOracle");
builder.Services.AddDbContext<ContextoBD>(options =>
    options.UseOracle(connectionString));

// 2. REGISTRO DE SERVICIOS DE NEGOCIO Y SESIÓN
builder.Services.AddScoped<UsuarioNegocio>();
builder.Services.AddScoped<TicketNegocio>();
builder.Services.AddScoped<ServicioSesion>();

// 3. CONFIGURACIÓN DE MUDBLAZOR (Versión 7/8 compatible)
builder.Services.AddMudServices();

// 4. RENDERIZADO INTERACTIVO Y SEGURIDAD
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => {
        options.LoginPath = "/login";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
    });

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacion>();

var app = builder.Build();

// 5. PIPELINE DE MIDDLEWARE
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
app.UseAuthentication();
app.UseAuthorization();

// MAPEO DE COMPONENTES (Soluciona Pantalla en Blanco y 404)
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ITSM.WEB.Client._Imports).Assembly);

app.Run();