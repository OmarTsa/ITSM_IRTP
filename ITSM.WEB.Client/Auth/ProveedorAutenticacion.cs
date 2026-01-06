using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using ITSM.Entidades.DTOs;

namespace ITSM.WEB.Client.Auth
{
    public class ProveedorAutenticacion : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;
        private readonly AuthenticationState _anonimo;

        public ProveedorAutenticacion(ILocalStorageService localStorage, HttpClient http)
        {
            _localStorage = localStorage;
            _http = http;
            _anonimo = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                Console.WriteLine("🔐 [Auth] Verificando autenticación...");

                // Timeout de 3 segundos para evitar deadlock
                var timeoutTask = Task.Delay(3000);
                var storageTask = _localStorage.GetItemAsync<SesionDto>("sesionUsuario").AsTask();

                var completedTask = await Task.WhenAny(storageTask, timeoutTask);

                if (completedTask == timeoutTask)
                {
                    Console.WriteLine("⏱️ [Auth] Timeout - localStorage no responde");
                    return _anonimo;
                }

                var sesionUsuario = await storageTask;

                if (sesionUsuario == null || string.IsNullOrWhiteSpace(sesionUsuario.Token))
                {
                    Console.WriteLine("⚠️ [Auth] No hay sesión activa");
                    return _anonimo;
                }

                Console.WriteLine($"✅ [Auth] Sesión encontrada: {sesionUsuario.Username}");

                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sesionUsuario.Token);

                var claims = ParseClaimsFromJwt(sesionUsuario.Token);
                var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

                return new AuthenticationState(user);
            }
            catch (JsonException ex)
            {
                // ⚠️ DATOS CORRUPTOS EN LOCALSTORAGE - LIMPIEZA AUTOMÁTICA
                Console.WriteLine($"⚠️ [Auth] JSON corrupto en localStorage: {ex.Message}");
                Console.WriteLine($"🧹 [Auth] Limpiando datos corruptos automáticamente...");

                try
                {
                    await _localStorage.RemoveItemAsync("sesionUsuario");
                    Console.WriteLine("✅ [Auth] LocalStorage limpio. Por favor, inicia sesión nuevamente.");
                }
                catch (Exception clearEx)
                {
                    Console.WriteLine($"❌ [Auth] Error al limpiar localStorage: {clearEx.Message}");
                }

                return _anonimo;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"⚠️ [Auth] LocalStorage no disponible (prerendering): {ex.Message}");
                return _anonimo;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ [Auth] Error: {ex.Message}");
                Console.WriteLine($"   StackTrace: {ex.StackTrace}");
                return _anonimo;
            }
        }

        public void NotificarUsuarioLogueado(string token)
        {
            Console.WriteLine("🔔 [Auth] Usuario logueado");
            var claims = ParseClaimsFromJwt(token);
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);
        }

        public void NotificarUsuarioDeslogueado()
        {
            Console.WriteLine("🔔 [Auth] Usuario deslogueado");
            var authState = Task.FromResult(_anonimo);
            NotifyAuthenticationStateChanged(authState);
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            try
            {
                var parts = jwt.Split('.');
                if (parts.Length < 2) return Enumerable.Empty<Claim>();

                var payload = parts[1];
                var jsonBytes = ParseBase64WithoutPadding(payload);

                using var doc = JsonDocument.Parse(jsonBytes);
                var claims = new List<Claim>();

                foreach (var prop in doc.RootElement.EnumerateObject())
                {
                    // Manejar arrays en JWT
                    if (prop.Value.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var item in prop.Value.EnumerateArray())
                        {
                            var value = item.ValueKind == JsonValueKind.String
                                ? item.GetString()
                                : item.ToString();

                            if (!string.IsNullOrEmpty(value))
                            {
                                claims.Add(new Claim(prop.Name, value));
                            }
                        }
                    }
                    else
                    {
                        var value = prop.Value.ValueKind == JsonValueKind.String
                            ? prop.Value.GetString()
                            : prop.Value.ToString();

                        if (!string.IsNullOrEmpty(value))
                        {
                            claims.Add(new Claim(prop.Name, value));
                        }
                    }
                }

                return claims;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error parseando JWT: {ex.Message}");
                return Enumerable.Empty<Claim>();
            }
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
