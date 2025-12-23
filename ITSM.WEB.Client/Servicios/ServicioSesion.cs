using System.Net.Http.Json;
using ITSM.WEB.Client.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace ITSM.WEB.Client.Servicios
{
    public class ServicioSesion
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;
        private readonly AuthenticationStateProvider _authStateProvider;

        public ServicioSesion(HttpClient http, IJSRuntime js, AuthenticationStateProvider authStateProvider)
        {
            _http = http;
            _js = js;
            _authStateProvider = authStateProvider;
        }

        // --- ESTE ES EL MÉTODO QUE FALTABA ---
        public async Task<bool> IniciarSesion(string email, string password)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/autenticacion/login", new { Email = email, Password = password });

                if (response.IsSuccessStatusCode)
                {
                    var loginResult = await response.Content.ReadFromJsonAsync<LoginResult>();
                    if (loginResult != null && !string.IsNullOrEmpty(loginResult.Token))
                    {
                        await _js.InvokeVoidAsync("localStorage.setItem", "authToken", loginResult.Token);
                        ((ProveedorAutenticacion)_authStateProvider).NotificarLogin(loginResult.Token);
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task CerrarSesion()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", "authToken");
            ((ProveedorAutenticacion)_authStateProvider).NotificarLogout();
        }

        private class LoginResult
        {
            public string Token { get; set; } = string.Empty;
        }
    }
}