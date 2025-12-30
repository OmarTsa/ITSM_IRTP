using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Authorization;
using ITSM.WEB.Client.Auth;
using ITSM.WEB.Client.Servicios;
using Blazored.LocalStorage;

var constructor = WebAssemblyHostBuilder.CreateDefault(args);

// ===== BLAZORED LOCALSTORAGE =====
constructor.Services.AddBlazoredLocalStorage();

// ===== MANEJADOR DE AUTORIZACIÓN PERSONALIZADO =====
constructor.Services.AddScoped<ManejadorAutorizacionPersonalizado>();

// ===== CLIENTE HTTP CON AUTENTICACIÓN JWT AUTOMÁTICA =====
constructor.Services.AddHttpClient("ITSM.WEB.API", cliente =>
{
    cliente.BaseAddress = new Uri(constructor.HostEnvironment.BaseAddress);
    cliente.DefaultRequestHeaders.Add("Accept", "application/json");
    cliente.Timeout = TimeSpan.FromSeconds(30);
})
.AddHttpMessageHandler<ManejadorAutorizacionPersonalizado>();

// Cliente HTTP por defecto (usa la configuración anterior)
constructor.Services.AddScoped(sp =>
{
    var fabricaClienteHttp = sp.GetRequiredService<IHttpClientFactory>();
    return fabricaClienteHttp.CreateClient("ITSM.WEB.API");
});

// ===== MUDBLAZOR =====
constructor.Services.AddMudServices(configuracion =>
{
    configuracion.SnackbarConfiguration.PositionClass = MudBlazor.Defaults.Classes.Position.BottomRight;
    configuracion.SnackbarConfiguration.MaxDisplayedSnackbars = 5;
    configuracion.SnackbarConfiguration.VisibleStateDuration = 4000;
});

// ===== AUTENTICACIÓN Y AUTORIZACIÓN =====
constructor.Services.AddAuthorizationCore();
constructor.Services.AddCascadingAuthenticationState();
constructor.Services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacion>();

// ===== SERVICIOS DE NEGOCIO =====
constructor.Services.AddScoped<IServicioSesion, ServicioSesion>();
constructor.Services.AddScoped<ITicketServicio, TicketServicio>();
constructor.Services.AddScoped<IUsuarioServicio, UsuarioServicio>();
constructor.Services.AddScoped<IInventarioServicio, InventarioServicio>();
constructor.Services.AddScoped<IDashboardServicio, DashboardServicio>();

// ===== LOGS DE INICIALIZACIÓN =====
Console.WriteLine("=====================================");
Console.WriteLine("🚀 ITSM - Cliente Blazor WebAssembly");
Console.WriteLine("=====================================");
Console.WriteLine($"🌐 URL Base: {constructor.HostEnvironment.BaseAddress}");
Console.WriteLine($"🔧 Entorno: {constructor.HostEnvironment.Environment}");
Console.WriteLine("✅ Servicios configurados correctamente");
Console.WriteLine("=====================================");

await constructor.Build().RunAsync();
