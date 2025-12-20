using Microsoft.AspNetCore.Mvc;
using ITSM.Negocio;
using ITSM.Entidades;
using System.Security.Claims; // Necesario para Claims
using Microsoft.AspNetCore.Authentication; // Necesario para Cookies
using Microsoft.AspNetCore.Authentication.Cookies; // Necesario para Cookies

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

            var usuario = await _usuarioNegocio.LoginAsync(request.NombreUsuario, request.Clave);

            if (usuario == null)
            {
                return Unauthorized(new { mensaje = "Credenciales incorrectas" });
            }

            // --- CREACIÓN DE LA COOKIE DE SESIÓN (OBLIGATORIO PARA BLAZOR SERVER) ---
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Username),
                new Claim(ClaimTypes.Role, usuario.IdRol == 1 ? "ADMIN" : "USUARIO"),
                // CORRECCIÓN CLAVE: Aquí guardamos el ID para que NuevoTicket lo encuentre
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(60)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            usuario.PasswordHash = "";

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