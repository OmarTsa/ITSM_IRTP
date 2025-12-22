using Microsoft.AspNetCore.Mvc;
using ITSM.Negocio;
using ITSM.Entidades;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ITSM.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        private readonly UsuarioNegocio _usuarioNegocio;

        public AutenticacionController(UsuarioNegocio usuarioNegocio)
        {
            _usuarioNegocio = usuarioNegocio;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] SolicitudAcceso request)
        {
            if (request == null || string.IsNullOrEmpty(request.NombreUsuario) || string.IsNullOrEmpty(request.Clave))
            {
                return BadRequest(new { mensaje = "Datos incompletos" });
            }

            // 1. Validar credenciales usando la capa de Negocio
            var usuario = await _usuarioNegocio.LoginAsync(request.NombreUsuario, request.Clave);

            if (usuario == null)
            {
                return Unauthorized(new { mensaje = "Credenciales incorrectas" });
            }

            // 2. Crear los Claims (Datos de la identidad del usuario)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Role, usuario.Rol?.Nombre ?? "Usuario"),
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(60)
            };

            // 3. Crear la Cookie de Sesión (Esta es la línea de autenticación)
            // IMPORTANTE: Esto requiere que en Program.cs hayas agregado .AddAuthentication().AddCookie()
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // 4. Limpiar datos sensibles antes de devolver al cliente
            usuario.Clave = "";

            return Ok(usuario);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { mensaje = "Sesión cerrada" });
        }
    }
}