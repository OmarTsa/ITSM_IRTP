using ITSM.WEB.Components;
using ITSM.Negocio;
using ITSM.Datos;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using ITSM.WEB.Client.Servicios;
using ITSM.WEB.Client.Auth;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// 1. CONFIGURACIÓN DE BLAZOR (Interactive Server + WebAssembly)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();

// 2. CONEXIÓN A BASE DE DATOS (Oracle)
builder.Services.AddDbContext<ContextoBD>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. REGISTRO DE CAPA DE NEGOCIO (Backend)
builder.Services.AddScoped<TicketNegocio>();
builder.Services.AddScoped<UsuarioNegocio>();
builder.Services.AddScoped<ActivoNegocio>();

// 4. REGISTRO DE SERVICIOS DEL CLIENTE (Para Pre-renderizado en Servidor)
// Nota: Se requiere HttpClient para que los servicios del cliente funcionen en el SSR
builder.Services.AddScoped(sp => new HttpClient
{
    // Usamos la IP configurada para asegurar consistencia en la red
    BaseAddress = new Uri("http://172.30.97.30:5244/")
});

builder.Services.AddScoped<TicketServicio>();
builder.Services.AddScoped<ServicioSesion>();
builder.Services.AddScoped<InventarioServicio>();
builder.Services.AddScoped<UsuarioServicio>();

// 5. CONFIGURACIÓN DE AUTENTICACIÓN Y ESTADO
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "AuthCookie_ITSM";
        options.LoginPath = "/login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

// Proveedor de estado de autenticación compartido
builder.Services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacion>();

builder.Services.AddControllers();

var app = builder.Build();

// 6. CONFIGURACIÓN DEL PIPELINE DE SOLICITUDES
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
app.MapStaticAssets();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ITSM.WEB.Client._Imports).Assembly);

app.Run();