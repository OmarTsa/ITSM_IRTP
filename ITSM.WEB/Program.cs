using ITSM.WEB.Components;
using ITSM.Negocio;
using ITSM.Datos;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using ITSM.WEB.Client.Servicios;
using ITSM.WEB.Client.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Routing;
using ITSM.WEB.Helpers;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Register a mutable EndpointDataSource early so AuthorizationPolicyCache and other
// services that depend on EndpointDataSource can be constructed safely during startup.
var mutableDataSource = new MutableEndpointDataSource();
builder.Services.AddSingleton<EndpointDataSource>(mutableDataSource);

// Register routing services
builder.Services.AddRouting();

// Blazor Interactive configuration
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();

// Database
builder.Services.AddDbContext<ContextoBD>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

// Business layer
builder.Services.AddScoped<TicketNegocio>();
builder.Services.AddScoped<UsuarioNegocio>();
builder.Services.AddScoped<ActivoNegocio>();

// Client services (HttpClient base address from config)
var apiBase = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5244/";
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBase) });

builder.Services.AddScoped<TicketServicio>();
builder.Services.AddScoped<ServicioSesion>();
builder.Services.AddScoped<InventarioServicio>();
builder.Services.AddScoped<UsuarioServicio>();

// Authentication & Authorization
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "AuthCookie_ITSM";
        options.LoginPath = "/login";
    });

builder.Services.AddAuthorization();

// Blazor authentication state provider (client side)
builder.Services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacion>();

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

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints and then populate the mutable endpoint data source with the created endpoints
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    // Map Razor components (no additional assemblies to avoid duplicates)
    endpoints.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode()
        .AddInteractiveWebAssemblyRenderMode();

    // Serve SPA fallback (Blazor WebAssembly client index.html)
    endpoints.MapFallbackToFile("index.html");

    // After the framework has created underlying endpoints, collect them from all sources
    var allDataSources = app.Services.GetServices<EndpointDataSource>().ToList();
    var collected = new List<Endpoint>();
    foreach (var ds in allDataSources)
    {
        try
        {
            collected.AddRange(ds.Endpoints ?? Array.Empty<Endpoint>());
        }
        catch { }
    }

    // Populate the mutable data source used during DI construction with the real endpoints
    mutableDataSource.SetEndpoints(collected);
});

app.Run();