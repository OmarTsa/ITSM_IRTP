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
        public async Task<IActionResult> Login([FromBody] SolicitudAcceso request)
        {
            // Validación básica de seguridad
            if (request == null || string.IsNullOrEmpty(request.NombreUsuario) || string.IsNullOrEmpty(request.Clave))
            {
                return BadRequest(new { mensaje = "Datos incompletos" });
            }

            // Llamada a la capa de negocio usando los datos correctos
            var usuario = await _usuarioNegocio.LoginAsync(request.NombreUsuario, request.Clave);

            if (usuario == null)
            {
                // Si falla, retornamos Unauthorized (401)
                return Unauthorized(new { mensaje = "Credenciales incorrectas" });
            }

            // Por seguridad, limpiamos la clave antes de enviarla de vuelta
            usuario.Clave = "";

            return Ok(usuario);
        }
    }
}