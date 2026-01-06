using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MudBlazor.Services;
using Microsoft.EntityFrameworkCore;
using ITSM.Datos;
using ITSM.Negocio;
using ITSM.WEB.Components;

var builder = WebApplication.CreateBuilder(args);

// =====================================================
// CONFIGURACIÓN DE SERVICIOS
// =====================================================

// DbContext para Oracle
builder.Services.AddDbContext<ContextoBD>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// Servicios de negocio
builder.Services.AddScoped<TicketNegocio>();
builder.Services.AddScoped<UsuarioNegocio>();

// JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("SecretKey no configurada");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddAuthorization();

// MudBlazor
builder.Services.AddMudServices();
builder.Services.AddControllers();

// Blazor
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// =====================================================
// CONFIGURACIÓN DEL PIPELINE HTTP - ORDEN CORRECTO
// =====================================================

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// IMPORTANTE: MapStaticAssets() ANTES de UseRouting()
app.MapStaticAssets();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

// ENDPOINTS AL FINAL
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ITSM.WEB.Client._Imports).Assembly);

app.MapControllers();

Console.WriteLine("✅ Servidor ITSM iniciado correctamente");
Console.WriteLine($"🔑 JWT configurado: Issuer={jwtSettings["Issuer"]}, Audience={jwtSettings["Audience"]}");
Console.WriteLine("🌐 CORS habilitado para desarrollo");

app.Run();



