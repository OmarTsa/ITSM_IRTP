using MudBlazor.Services;
using Microsoft.EntityFrameworkCore;
using ITSM.Datos;
using ITSM.Negocio;
using ITSM.WEB.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization; // Indispensable para AuthenticationStateProvider
using Microsoft.AspNetCore.Authentication.Cookies;
using ITSM.WEB.Client.Auth; // Asegúrate que el namespace coincida con tu ProveedorAutenticacion.cs

var builder = WebApplication.CreateBuilder(args);

// 1. CONFIGURACIÓN DE BASE DE DATOS
var connectionString = builder.Configuration.GetConnectionString("ConexionOracle");
builder.Services.AddDbContext<ContextoBD>(options => options.UseOracle(connectionString));

// 2. SERVICIOS DE NEGOCIO
builder.Services.AddScoped<UsuarioNegocio>();
builder.Services.AddScoped<TicketNegocio>();

// 3. SERVICIOS DE INTERFAZ
builder.Services.AddMudServices();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// 4. SEGURIDAD Y ESTADO DE AUTENTICACIÓN
// Esto resuelve el error "AuthenticationStateProvider no se encontró"
builder.Services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacion>();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// 5. MIDDLEWARES
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

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ITSM.WEB.Client._Imports).Assembly);

app.Run();