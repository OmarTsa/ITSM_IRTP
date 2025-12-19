using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using ITSM.WEB.Client.Servicios;
using ITSM.Entidades;

namespace ITSM.WEB.Client.Auth
{
    public class ProveedorAutenticacion : AuthenticationStateProvider
    {
        private readonly ServicioSesion _servicioSesion;
        private ClaimsPrincipal _anonimo = new ClaimsPrincipal(new ClaimsIdentity());

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
                new Claim("NombreCompleto", usuario.NombreCompleto)
            };

            var identidad = new ClaimsIdentity(claims, "apiauth");
            return new AuthenticationState(new ClaimsPrincipal(identidad));
        }

        public void NotificarLogin(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Role, usuario.Rol ?? "Usuario")
            };
            var identidad = new ClaimsIdentity(claims, "apiauth");
            var userPrincipal = new ClaimsPrincipal(identidad);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(userPrincipal)));
        }

        public void NotificarLogout()
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonimo)));
        }
    }
}