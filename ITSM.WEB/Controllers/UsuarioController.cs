using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ITSM.Negocio;
using ITSM.Entidades;

namespace ITSM.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioNegocio _usuarioNegocio;

        public UsuarioController(UsuarioNegocio usuarioNegocio)
        {
            _usuarioNegocio = usuarioNegocio;
        }

        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> Listar()
        {
            var lista = await _usuarioNegocio.ListarUsuariosAsync();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> Obtener(int id)
        {
            var usuario = await _usuarioNegocio.ObtenerPorIdAsync(id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Usuario usuario)
        {
            string passwordInicial = "123456";
            try
            {
                await _usuarioNegocio.RegistrarUsuarioAsync(usuario, passwordInicial);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] Usuario usuario)
        {
            try
            {
                await _usuarioNegocio.ActualizarUsuarioAsync(usuario);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _usuarioNegocio.DarDeBajaAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("roles")]
        public async Task<ActionResult<List<Rol>>> ListarRoles()
        {
            var roles = await _usuarioNegocio.ListarRolesActivosAsync();
            return Ok(roles);
        }

        [HttpGet("areas")]
        public async Task<ActionResult<List<Area>>> ListarAreas()
        {
            var areas = await _usuarioNegocio.ListarAreasAsync();
            return Ok(areas);
        }

        // NUEVO ENDPOINT: Cambiar contraseña
        [HttpPost("cambiar-password")]
        public async Task<IActionResult> CambiarPassword([FromBody] CambiarPasswordRequest request)
        {
            try
            {
                if (request.IdUsuario <= 0)
                    return BadRequest("ID de usuario inválido");

                if (string.IsNullOrWhiteSpace(request.NuevaPassword))
                    return BadRequest("La contraseña no puede estar vacía");

                if (request.NuevaPassword.Length < 6)
                    return BadRequest("La contraseña debe tener al menos 6 caracteres");

                await _usuarioNegocio.CambiarContrasenaAsync(request.IdUsuario, request.NuevaPassword);
                return Ok(new { mensaje = "Contraseña actualizada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

    // Clase auxiliar para el request de cambiar contraseña
    public class CambiarPasswordRequest
    {
        public int IdUsuario { get; set; }
        public string NuevaPassword { get; set; } = string.Empty;
    }
}
