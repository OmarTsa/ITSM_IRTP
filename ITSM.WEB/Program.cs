using ITSM.Datos;
using ITSM.Negocio;
using ITSM.WEB.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services; // NECESARIO PARA DISEÑO
using Blazored.LocalStorage; // NECESARIO PARA LOGIN
using ITSM.WEB.Client.Servicios; // NECESARIO PARA LOGIN
using ITSM.WEB.Client.Auth; // NECESARIO PARA LOGIN
using Microsoft.AspNetCore.Components.Authorization; // NECESARIO PARA LOGIN

var builder = WebApplication.CreateBuilder(args);

// 1. Servicios Base de Blazor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// 2. Servicios de Diseño (MudBlazor) - VITAL
builder.Services.AddMudServices();

// 3. Base de Datos
builder.Services.AddDbContext<ContextoBD>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

// 4. Lógica de Negocio (Servidor)
builder.Services.AddScoped<UsuarioNegocio>();
builder.Services.AddScoped<TicketNegocio>();
builder.Services.AddScoped<ActivoNegocio>();

// 5. Servicios Cliente en el Servidor (VITAL PARA QUE NO EXPLOTE AL CARGAR)
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacion>();
builder.Services.AddScoped<ServicioSesion>();
builder.Services.AddScoped<UsuarioServicio>();
builder.Services.AddScoped<TicketServicio>();
builder.Services.AddScoped<InventarioServicio>();

// Configurar HttpClient para que el servidor pueda "hablarse a sí mismo"
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7233") // Asegúrate que este puerto coincida con tu launchSettings.json
});

// 6. Controladores
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
app.UseAntiforgery();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ITSM.WEB.Client._Imports).Assembly);

app.Run();