using Blazored.LocalStorage;
using ITSM.WEB.Client.Auth;
using ITSM.WEB.Client.Servicios;
using Microsoft.AspNetCore.Components.Authorization;

// ...
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacion>();
builder.Services.AddScoped<ServicioSesion>();

// Asegurar que HttpClient apunte al servidor correcto
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });