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
    }
}
