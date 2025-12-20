using MudBlazor.Services;
using Microsoft.EntityFrameworkCore;
using ITSM.Datos;
using ITSM.Negocio;
using ITSM.WEB.Components;
using Microsoft.AspNetCore.Components;
using ITSM.WEB.Client.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// 1. CONEXIÓN A ORACLE
var connectionString = builder.Configuration.GetConnectionString("ConexionOracle");
builder.Services.AddDbContext<ContextoBD>(options =>
    options.UseOracle(connectionString));

// 2. INYECCIÓN DE CAPA DE NEGOCIO (Acceso directo para Blazor Server)
builder.Services.AddScoped<UsuarioNegocio>();
builder.Services.AddScoped<TicketNegocio>();

// 3. CONFIGURACIÓN DE INTERFAZ MUDBLAZOR
builder.Services.AddMudServices();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); // Habilita el modo profesional de servidor

// 4. SEGURIDAD Y ESTADO DE AUTENTICACIÓN
builder.Services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacion>();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// 5. PIPELINE DE MIDDLEWARE
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); // Mantenemos soporte para controladores si se requieren

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode(); // Renderizado en servidor para máxima velocidad

app.Run();