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
                // CORRECCIÓN: Usamos NameIdentifier en lugar de "IdUsuario"
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString())
            };

            if (!string.IsNullOrEmpty(usuario.NombreCompleto))
            {
                claims.Add(new Claim("NombreCompleto", usuario.NombreCompleto));
            }

            var identidad = new ClaimsIdentity(claims, "JwtAuth");
            return new AuthenticationState(new ClaimsPrincipal(identidad));
        }

        public async Task NotificarLogin(Usuario usuario)
        {
            await _servicioSesion.GuardarUsuario(usuario);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Role, usuario.Rol ?? "Usuario"),
                // CORRECCIÓN: Aquí también usamos NameIdentifier
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString())
            };

            var identidad = new ClaimsIdentity(claims, "JwtAuth");
            var userPrincipal = new ClaimsPrincipal(identidad);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(userPrincipal)));
        }

        public async Task NotificarLogout()
        {
            await _servicioSesion.CerrarSesion();
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonimo)));
        }
    }
}