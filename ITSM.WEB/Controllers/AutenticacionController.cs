using Microsoft.AspNetCore.Mvc;
using ITSM.Negocio;
using ITSM.Entidades;

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
        public async Task<IActionResult> Login([FromBody] SolicitudAcceso solicitud)
        {
            // El método Login debe incluir el .Include(u => u.Rol)
            var usuario = await _usuarioNegocio.Login(solicitud.Email, solicitud.Password);

            if (usuario != null)
            {
                return Ok(usuario);
            }

            return Unauthorized("Usuario o contraseña incorrectos");
        }
    }

    public class SolicitudAcceso
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}