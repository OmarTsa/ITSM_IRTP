using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ITSM.Negocio;
using ITSM.Entidades;

namespace ITSM.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Protegemos toda la API para que solo usuarios logueados accedan
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioNegocio _usuarioNegocio;

        public UsuarioController(UsuarioNegocio usuarioNegocio)
        {
            _usuarioNegocio = usuarioNegocio;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var lista = await _usuarioNegocio.ListarUsuariosAsync();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var usuario = await _usuarioNegocio.ObtenerPorIdAsync(id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Usuario usuario)
        {
            // Usamos una contraseña temporal si viene vacía, o la que mande el admin
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

        // Endpoints para llenar los combos
        [HttpGet("roles")]
        public async Task<IActionResult> ListarRoles()
        {
            var roles = await _usuarioNegocio.ListarRolesActivosAsync();
            return Ok(roles);
        }

        [HttpGet("areas")]
        public async Task<IActionResult> ListarAreas()
        {
            var areas = await _usuarioNegocio.ListarAreasAsync();
            return Ok(areas);
        }
    }
}