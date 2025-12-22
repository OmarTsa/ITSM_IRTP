using ITSM.WEB.Components;
using ITSM.Negocio;
using ITSM.Datos;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Microsoft.AspNetCore.Authentication.Cookies; // <--- AGREGAR ESTO

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar Blazor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();

// 2. Conexión a Base de Datos
builder.Services.AddDbContext<ContextoBD>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Inyección de Negocio
builder.Services.AddScoped<TicketNegocio>();
builder.Services.AddScoped<UsuarioNegocio>();

// --- NUEVO: CONFIGURACIÓN DE COOKIES (NECESARIO PARA EL LOGIN) ---
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "AuthCookie_ITSM";
        options.LoginPath = "/login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });
builder.Services.AddAuthorization(); // Habilitar seguridad
// ---------------------------------------------------------------

// 4. Habilitar APIs
builder.Services.AddControllers();
builder.Services.AddHttpClient();

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
app.MapStaticAssets(); // Mantener para .NET 9/10
app.UseAntiforgery();

// --- NUEVO: ACTIVAR LOS MIDDLEWARES DE SEGURIDAD ---
// (Deben ir en este orden exacto: Auth -> Authorization)
app.UseAuthentication();
app.UseAuthorization();
// ---------------------------------------------------

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ITSM.WEB.Client._Imports).Assembly);

app.Run();