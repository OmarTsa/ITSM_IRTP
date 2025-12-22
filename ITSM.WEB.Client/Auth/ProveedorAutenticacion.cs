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

            // Crear los Claims (Datos de la credencial)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                
                // --- CORREGIDO: Accedemos a usuario.Rol?.Nombre ---
                // Si usuario.Rol es nulo, usa "Usuario" por defecto
                new Claim(ClaimTypes.Role, usuario.Rol?.Nombre ?? "Usuario"),

                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString())
            };

            // Propiedad calculada en Usuario.cs (NombreCompleto)
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

                // --- CORREGIDO: Lo mismo aquí ---
                new Claim(ClaimTypes.Role, usuario.Rol?.Nombre ?? "Usuario"),

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