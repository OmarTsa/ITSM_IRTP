using Microsoft.AspNetCore.Mvc;
using ITSM.Negocio;
using ITSM.Entidades;

namespace ITSM.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioNegocio _usuarioNegocio;

        public UsuarioController(UsuarioNegocio usuarioNegocio)
        {
            _usuarioNegocio = usuarioNegocio;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _usuarioNegocio.ListarUsuarios());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var usuario = await _usuarioNegocio.ObtenerPorId(id);
            return usuario != null ? Ok(usuario) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Usuario usuario)
        {
            await _usuarioNegocio.GuardarUsuario(usuario);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Usuario usuario)
        {
            if (id != usuario.IdUsuario) return BadRequest();
            await _usuarioNegocio.GuardarUsuario(usuario);
            return Ok();
        }
    }
}