using ITSM.WEB.Components;
using ITSM.Negocio;
using ITSM.Datos;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
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

// --- 4. CONFIGURACIÓN DEL CLIENTE ---
builder.Services.AddScoped(sp => new HttpClient
{
    // AQUÍ MANTENEMOS TU IP REAL DE LA RED
    // Esto asegura que el cliente WebAssembly sepa dónde buscar la API
    BaseAddress = new Uri("http://172.30.97.30:5244/")
});

builder.Services.AddScoped<TicketServicio>();
builder.Services.AddScoped<ServicioSesion>();
// -------------------------------------------------------------------

// 5. Configuración de Cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "AuthCookie_ITSM";
        options.LoginPath = "/login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

builder.Services.AddAuthorization();
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

// IMPORTANTE: .Run() toma la configuración del launchSettings.json
app.Run();