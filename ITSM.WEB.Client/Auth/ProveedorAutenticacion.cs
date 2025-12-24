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
                // INTENTO DE LEER LOCALSTORAGE
                // Si estamos en el servidor (prerendering), esto fallará.
                // Capturamos el error y devolvemos "Anónimo" para que la app cargue.
                var sesionUsuario = await _localStorage.GetItemAsync<SesionDto>("sesionUsuario");

                if (sesionUsuario == null)
                    return _anonimo;

                // Si llegamos aquí, es que estamos en el navegador y hay sesión
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sesionUsuario.Token);

                var claims = ParseClaimsFromJwt(sesionUsuario.Token);
                var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

                return new AuthenticationState(user);
            }
            catch (InvalidOperationException)
            {
                // Estamos en el servidor (Prerendering), no hay JS todavía.
                // Devolvemos anónimo y esperamos a que el cliente se conecte.
                return _anonimo;
            }
            catch (Exception)
            {
                // Cualquier otro error de lectura
                return _anonimo;
            }
        }

        public void NotificarUsuarioLogueado(string token)
        {
            var claims = ParseClaimsFromJwt(token);
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);
        }

        public void NotificarUsuarioDeslogueado()
        {
            var authState = Task.FromResult(_anonimo);
            NotifyAuthenticationStateChanged(authState);
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var parts = jwt.Split('.');
            if (parts.Length < 2) return Enumerable.Empty<Claim>();

            var payload = parts[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            if (keyValuePairs == null) return Enumerable.Empty<Claim>();

            // CORRECCIÓN CS8604: Usamos "??" para evitar pasar null al constructor de Claim
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value?.ToString() ?? ""));
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