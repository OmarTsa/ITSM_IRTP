using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using ITSM.WEB.Client.Servicios;
using ITSM.Entidades;

namespace ITSM.WEB.Client.Auth
{
    public class ProveedorAutenticacion : AuthenticationStateProvider
    {
        private readonly ServicioSesion _servicioSesion;
        private readonly ClaimsPrincipal _anonimo = new ClaimsPrincipal(new ClaimsIdentity());

        public ProveedorAutenticacion(ServicioSesion servicioSesion)
        {
            _servicioSesion = servicioSesion;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var usuario = await _servicioSesion.ObtenerUsuario();

            if (usuario == null)
                return new AuthenticationState(_anonimo);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Role, usuario.Rol ?? "Usuario"),
                new Claim("IdUsuario", usuario.IdUsuario.ToString())
            };

            if (!string.IsNullOrEmpty(usuario.NombreCompleto))
            {
                claims.Add(new Claim("NombreCompleto", usuario.NombreCompleto));
            }

            var identidad = new ClaimsIdentity(claims, "JwtAuth");
            return new AuthenticationState(new ClaimsPrincipal(identidad));
        }

        // CORRECCIÓN: Ahora devuelve 'Task' para poder usar 'await'
        public async Task NotificarLogin(Usuario usuario)
        {
            // 1. Guardamos la sesión (usando el método que acabamos de definir)
            await _servicioSesion.GuardarUsuario(usuario);

            // 2. Construimos la identidad
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Role, usuario.Rol ?? "Usuario"),
                new Claim("IdUsuario", usuario.IdUsuario.ToString())
            };

            var identidad = new ClaimsIdentity(claims, "JwtAuth");
            var userPrincipal = new ClaimsPrincipal(identidad);

            // 3. Notificamos a Blazor
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(userPrincipal)));
        }

        public async Task NotificarLogout()
        {
            await _servicioSesion.CerrarSesion();
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonimo)));
        }
    }
}