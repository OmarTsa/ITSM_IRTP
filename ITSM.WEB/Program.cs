using ITSM.WEB.Components;
using ITSM.Negocio;
using ITSM.Datos;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
// USINGS NECESARIOS PARA EL CLIENTE
using ITSM.WEB.Client.Servicios;
using ITSM.WEB.Client.Auth;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar Blazor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();

// 2. Conexión a Base de Datos
builder.Services.AddDbContext<ContextoBD>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Inyección de Negocio (Servidor)
builder.Services.AddScoped<TicketNegocio>();
builder.Services.AddScoped<UsuarioNegocio>();

// --- 4. CONFIGURACIÓN DEL CLIENTE EN EL SERVIDOR (CORREGIDO) ---
// Registramos HttpClient para que el servidor pueda "auto-llamar" a su API.
// IMPORTANTE: Usamos HTTP (Puerto 5244) para evitar errores de certificado SSL.
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5244/")
});

// Registramos los servicios del Cliente para que funcionen en el Servidor
builder.Services.AddScoped<TicketServicio>();
builder.Services.AddScoped<ServicioSesion>();
// -------------------------------------------------------------------

// 5. Configuración de Cookies (Autenticación)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "AuthCookie_ITSM";
        options.LoginPath = "/login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

builder.Services.AddAuthorization();

// 6. Habilitar APIs
builder.Services.AddControllers();

var app = builder.Build();

// Pipeline HTTP
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
app.MapStaticAssets(); // Configuración para .NET 10
app.UseAntiforgery();

// 7. Activar Seguridad (Orden Importante)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ITSM.WEB.Client._Imports).Assembly);

app.Run();