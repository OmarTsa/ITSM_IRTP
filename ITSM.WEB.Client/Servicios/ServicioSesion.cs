using ITSM.Entidades.DTOs;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using ITSM.WEB.Client.Auth;
using Microsoft.AspNetCore.Components.Authorization;

namespace ITSM.WEB.Client.Servicios
{
    public class ServicioSesion
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;

        public ServicioSesion(HttpClient http, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
        {
            _http = http;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }

        public async Task<SesionDto> IniciarSesion(LoginDto login)
        {
            var response = await _http.PostAsJsonAsync("api/autenticacion/login", login);

            if (response.IsSuccessStatusCode)
            {
                var sesion = await response.Content.ReadFromJsonAsync<SesionDto>();

                // Guardamos el token y los datos
                await _localStorage.SetItemAsync("sesionUsuario", sesion);

                // Notificamos al proveedor de autenticación que el estado cambió
                ((ProveedorAutenticacion)_authStateProvider).NotificarUsuarioLogueado(sesion.Token);

                return sesion;
            }
            else
            {
                return null;
            }
        }

        public async Task CerrarSesion()
        {
            await _localStorage.RemoveItemAsync("sesionUsuario");
            ((ProveedorAutenticacion)_authStateProvider).NotificarUsuarioDeslogueado();
        }
    }
}