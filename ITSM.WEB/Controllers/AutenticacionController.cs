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
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // CORRECCIÓN: Usamos 'LoginAsync' que es el que existe en tu Negocio
            var usuario = await _usuarioNegocio.LoginAsync(request.Usuario, request.Clave);

            if (usuario == null)
            {
                return Unauthorized(new { mensaje = "Credenciales incorrectas" });
            }

            return Ok(usuario);
        }

        // Clase auxiliar para recibir los datos del JSON
        public class LoginRequest
        {
            public string Usuario { get; set; } = string.Empty;
            public string Clave { get; set; } = string.Empty;
        }
    }
}