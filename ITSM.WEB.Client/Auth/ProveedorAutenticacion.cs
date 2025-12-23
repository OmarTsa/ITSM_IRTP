using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
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
            try
            {
                var sesionUsuario = await _servicioSesion.ObtenerSesion();

                if (sesionUsuario == null)
                    return new AuthenticationState(_anonimo);

                return new AuthenticationState(CrearClaimsPrincipal(sesionUsuario));
            }
            catch
            {
                return new AuthenticationState(_anonimo);
            }
        }

        public async Task IniciarSesion(Usuario sesionUsuario)
        {
            await _servicioSesion.GuardarSesion(sesionUsuario);
            var claimsPrincipal = CrearClaimsPrincipal(sesionUsuario);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public async Task CerrarSesion()
        {
            await _servicioSesion.LimpiarSesion();
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonimo)));
        }

        private ClaimsPrincipal CrearClaimsPrincipal(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, usuario.Username ?? ""),
                new Claim(ClaimTypes.Email, usuario.Email ?? ""),
                new Claim(ClaimTypes.Role, usuario.Rol?.Nombre ?? "USUARIO_FINAL")
            };

            return new ClaimsPrincipal(new ClaimsIdentity(claims, "JwtAuth"));
        }
    }
}