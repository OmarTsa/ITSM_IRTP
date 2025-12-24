using ITSM.Datos;
using ITSM.Negocio;
using ITSM.WEB.Components;
using Microsoft.EntityFrameworkCore;
using Blazored.LocalStorage;
using ITSM.WEB.Client.Servicios;
using ITSM.WEB.Client.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
// Nuevos usings necesarios para JWT
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Servicios de Blazor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();

// 2. Base de Datos
builder.Services.AddDbContext<ContextoBD>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Servicios de Negocio
builder.Services.AddScoped<UsuarioNegocio>();
builder.Services.AddScoped<TicketNegocio>();
builder.Services.AddScoped<ActivoNegocio>();

// 4. Servicios Cliente (Espejo para renderizado servidor)
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacion>();
builder.Services.AddScoped<ServicioSesion>();
builder.Services.AddScoped<UsuarioServicio>();
builder.Services.AddScoped<TicketServicio>();
builder.Services.AddScoped<InventarioServicio>();

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7233") // Asegúrate que este puerto sea correcto
});

builder.Services.AddControllers();

// ==============================================================================
// --- NUEVO: CONFIGURACIÓN DE SEGURIDAD JWT (SOLUCIÓN AL ERROR DE AUTH) ---
// ==============================================================================
var keyString = builder.Configuration["Jwt:Key"] ?? "ClaveSecretaSuperSeguraParaTuTesis2025IRTP";
var keyBytes = Encoding.UTF8.GetBytes(keyString);

builder.Services.AddAuthentication(options =>
{
    // Definimos JWT como el esquema por defecto para autenticación
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Solo para desarrollo
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = false, // Simplificado para desarrollo
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});
// ==============================================================================

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

// MapStaticAssets para .NET 10
app.MapStaticAssets();
app.UseStaticFiles();

app.UseAntiforgery();

// --- ORDEN IMPORTANTE DE MIDDLEWARE ---
app.UseAuthentication(); // 1. ¿Quién eres? (Lee el Token)
app.UseAuthorization();  // 2. ¿Qué puedes hacer? (Revisa Roles)

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ITSM.WEB.Client._Imports).Assembly);

app.Run();