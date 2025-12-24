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
            var sesionUsuario = await _localStorage.GetItemAsync<SesionDto>("sesionUsuario");

            if (sesionUsuario == null)
                return _anonimo;

            // Inyectar el token en las cabeceras HTTP para futuras peticiones
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sesionUsuario.Token);

            var claims = ParseClaimsFromJwt(sesionUsuario.Token);
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

            return new AuthenticationState(user);
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

        // Método auxiliar para leer el Token sin librerías externas pesadas
        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
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