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

// 1. BASE DE DATOS
var connectionString = builder.Configuration.GetConnectionString("ConexionOracle");
builder.Services.AddDbContext<ContextoBD>(options =>
    options.UseOracle(connectionString));

// 2. SERVICIOS DE NEGOCIO Y SESIÓN
builder.Services.AddScoped<UsuarioNegocio>();
builder.Services.AddScoped<TicketNegocio>();
builder.Services.AddScoped<ServicioSesion>(); //

// 3. MUD BLAZOR Y COMPONENTES
builder.Services.AddMudServices();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); // Solo servidor para evitar errores 404 de WASM

// 4. SEGURIDAD
builder.Services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacion>(); //
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => {
        options.LoginPath = "/login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
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
app.UseStaticFiles(); // Importante para cargar MudBlazor CSS/JS
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

// 6. MAPEO DE RUTAS (Solución al 404)
// Agregamos AdditionalAssemblies para que encuentre las páginas en el proyecto Client
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(ITSM.WEB.Client._Imports).Assembly); //

app.Run();