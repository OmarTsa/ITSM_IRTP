using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // <--- NECESARIO PARA LA SEGURIDAD
using ITSM.Negocio;
using ITSM.Entidades;

namespace ITSM.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // <--- ESTO PROTEGE TUS DATOS DE ATAQUES EXTERNOS
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioNegocio _usuarioNegocio;

        public UsuarioController(UsuarioNegocio usuarioNegocio)
        {
            _usuarioNegocio = usuarioNegocio;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> ObtenerTodos()
        {
            try
            {
                var lista = await _usuarioNegocio.ListarUsuarios();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al consultar usuarios: {ex.Message}");
            }
        }
    }
}