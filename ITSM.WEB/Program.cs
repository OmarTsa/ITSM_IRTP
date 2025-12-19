using MudBlazor.Services;
using Microsoft.EntityFrameworkCore;
using ITSM.Datos;
using ITSM.Negocio;
using ITSM.WEB.Components;
using Microsoft.AspNetCore.Components;
// 1. AGREGA ESTOS NAMESPACES
using ITSM.WEB.Client.Servicios; // <--- NUEVO
using ITSM.WEB.Client.Auth;      // <--- NUEVO
using Microsoft.AspNetCore.Components.Authorization; // <--- NUEVO

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

// --- HTTP CLIENT (Para el servidor) ---
builder.Services.AddScoped(sp =>
{
    var navigation = sp.GetRequiredService<NavigationManager>();
    return new HttpClient { BaseAddress = new Uri(navigation.BaseUri) };
});

// --- SEGURIDAD Y SESIÓN (FALTABA ESTO EN EL SERVIDOR) ---
// Registramos los mismos servicios que en el cliente para que el pre-renderizado funcione
builder.Services.AddScoped<ServicioSesion>(); // <--- NUEVO
builder.Services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacion>(); // <--- NUEVO
builder.Services.AddAuthorizationCore(); // <--- NUEVO

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

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ITSM.WEB.Client._Imports).Assembly);

app.Run();