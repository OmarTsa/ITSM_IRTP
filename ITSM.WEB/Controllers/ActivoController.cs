using Microsoft.AspNetCore.Mvc;
using ITSM.Negocio;
using ITSM.Entidades;
using Microsoft.Extensions.Logging;

namespace ITSM.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivoController : ControllerBase
    {
        private readonly ActivoNegocio _activoNegocio;
        private readonly ILogger<ActivoController> _logger;

        public ActivoController(ActivoNegocio activoNegocio, ILogger<ActivoController> logger)
        {
            _activoNegocio = activoNegocio;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var activos = await _activoNegocio.ListarActivos();
                return Ok(activos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listando activos");
                return StatusCode(500, "Ocurrió un error al obtener los activos");
            }
        }

        [HttpGet("usuario/{id}")]
        public async Task<IActionResult> GetPorUsuario(int id)
        {
            try
            {
                var lista = await _activoNegocio.ListarPorUsuario(id);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listando activos por usuario {UsuarioId}", id);
                return StatusCode(500, "Ocurrió un error al obtener los activos del usuario");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var activo = await _activoNegocio.ObtenerPorId(id);
                return activo != null ? Ok(activo) : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo activo {ActivoId}", id);
                return StatusCode(500, "Ocurrió un error al obtener el activo");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Activo activo)
        {
            try
            {
                await _activoNegocio.GuardarActivo(activo);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando activo");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Activo activo)
        {
            if (id != activo.IdActivo) return BadRequest("El id del recurso no coincide");

            try
            {
                await _activoNegocio.GuardarActivo(activo);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando activo {ActivoId}", id);
                return BadRequest(ex.Message);
            }
        }

        // TIPOS
        [HttpGet("tipos")]
        public async Task<IActionResult> GetTipos()
        {
            try
            {
                var tipos = await _activoNegocio.ListarTipos();
                return Ok(tipos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listando tipos de activos");
                return StatusCode(500, "Ocurrió un error al obtener los tipos de activos");
            }
        }

        [HttpPost("tipos")]
        public async Task<IActionResult> PostTipo([FromBody] TipoActivo tipo)
        {
            try
            {
                await _activoNegocio.GuardarTipo(tipo);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando tipo de activo");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("tipos/{id}")]
        public async Task<IActionResult> PutTipo(int id, [FromBody] TipoActivo tipo)
        {
            if (id != tipo.IdTipo) return BadRequest("El id del recurso no coincide");

            try
            {
                await _activoNegocio.GuardarTipo(tipo);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando tipo de activo {TipoId}", id);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("tipos/{id}")]
        public async Task<IActionResult> DeleteTipo(int id)
        {
            try
            {
                await _activoNegocio.EliminarTipo(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando tipo de activo {TipoId}", id);
                return BadRequest(ex.Message);
            }
        }
    }
}