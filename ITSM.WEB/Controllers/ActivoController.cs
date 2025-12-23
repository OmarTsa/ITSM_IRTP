using Microsoft.AspNetCore.Mvc;
using ITSM.Negocio;
using ITSM.Entidades;

namespace ITSM.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivoController : ControllerBase
    {
        private readonly ActivoNegocio _activoNegocio;

        public ActivoController(ActivoNegocio activoNegocio)
        {
            _activoNegocio = activoNegocio;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _activoNegocio.ListarActivos());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var activo = await _activoNegocio.ObtenerPorId(id);
            return activo != null ? Ok(activo) : NotFound();
        }

        [HttpGet("usuario/{idUsuario}")]
        public async Task<IActionResult> GetPorUsuario(int idUsuario)
        {
            var lista = await _activoNegocio.ListarPorUsuario(idUsuario);
            return Ok(lista);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Activo activo)
        {
            await _activoNegocio.GuardarActivo(activo);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Activo activo)
        {
            if (id != activo.IdActivo) return BadRequest();
            await _activoNegocio.GuardarActivo(activo);
            return Ok();
        }

        [HttpGet("tipos")]
        public async Task<IActionResult> GetTipos() => Ok(await _activoNegocio.ListarTipos());

        [HttpPost("tipos")]
        public async Task<IActionResult> PostTipo([FromBody] TipoActivo tipo)
        {
            try
            {
                await _activoNegocio.GuardarTipo(tipo);
                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut("tipos/{id}")]
        public async Task<IActionResult> PutTipo(int id, [FromBody] TipoActivo tipo)
        {
            if (id != tipo.IdTipo) return BadRequest();
            await _activoNegocio.GuardarTipo(tipo);
            return Ok();
        }

        [HttpDelete("tipos/{id}")]
        public async Task<IActionResult> DeleteTipo(int id)
        {
            try
            {
                await _activoNegocio.EliminarTipo(id);
                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}