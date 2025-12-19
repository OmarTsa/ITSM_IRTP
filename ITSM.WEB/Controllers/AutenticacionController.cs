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

        [HttpPost("ingresar")]
        public async Task<IActionResult> Ingresar([FromBody] SolicitudAcceso solicitud)
        {
            // Validamos contra la base de datos
            var usuario = await _usuarioNegocio.ValidarAccesoReal(solicitud.NombreUsuario, solicitud.Clave);

            if (usuario != null)
            {
                // Login exitoso: devolvemos los datos del usuario
                return Ok(usuario);
            }
            else
            {
                // Login fallido
                return Unauthorized(new { mensaje = "Credenciales incorrectas" });
            }
        }
    }
}