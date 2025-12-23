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

// 1. Configuración de Blazor (Interactive Server + WebAssembly)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();

// 2. Conexión a Base de Datos
builder.Services.AddDbContext<ContextoBD>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Registro de Capa de Negocio (Backend)
builder.Services.AddScoped<TicketNegocio>();
builder.Services.AddScoped<UsuarioNegocio>();
builder.Services.AddScoped<ActivoNegocio>();

// 4. Registro de Servicios del Cliente (Vital para que Inventario.razor funcione en el servidor)
builder.Services.AddScoped(sp => new HttpClient
{
    // Asegúrate de que este puerto sea el que usas en ejecución
    BaseAddress = new Uri("http://172.30.97.30:5244/")
});

builder.Services.AddScoped<TicketServicio>();
builder.Services.AddScoped<ServicioSesion>();
builder.Services.AddScoped<InventarioServicio>();
builder.Services.AddScoped<UsuarioServicio>();

// 5. Configuración de Autenticación
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "AuthCookie_ITSM";
        options.LoginPath = "/login";
    });

builder.Services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacion>();

builder.Services.AddControllers();

var app = builder.Build();

// 6. Pipeline de Solicitudes
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

// CORRECCIÓN CRÍTICA: Mapeo de componentes y asambleas adicionales
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ITSM.WEB.Client._Imports).Assembly);

app.Run();