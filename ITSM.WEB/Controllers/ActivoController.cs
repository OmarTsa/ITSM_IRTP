using Microsoft.AspNetCore.Mvc;
using ITSM.Negocio;
using ITSM.Entidades;
using Microsoft.AspNetCore.Authorization;

namespace ITSM.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ActivoController : ControllerBase
    {
        private readonly ActivoNegocio _activoNegocio;

        public ActivoController(ActivoNegocio activoNegocio)
        {
            _activoNegocio = activoNegocio;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            // CORREGIDO: Coincide con ActivoNegocio.cs
            var lista = await _activoNegocio.ListarActivosAsync();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var activo = await _activoNegocio.ObtenerPorIdAsync(id);
            if (activo == null) return NotFound();
            return Ok(activo);
        }

        [HttpGet("usuario/{idUsuario}")]
        public async Task<IActionResult> ListarPorUsuario(int idUsuario)
        {
            var lista = await _activoNegocio.ListarActivosPorUsuarioAsync(idUsuario);
            return Ok(lista);
        }

        [HttpPost]
        public async Task<IActionResult> Guardar([FromBody] Activo activo)
        {
            try
            {
                await _activoNegocio.GuardarActivoAsync(activo);
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
                await _activoNegocio.EliminarActivoAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // --- TIPOS DE ACTIVO ---

        [HttpGet("tipos")]
        public async Task<IActionResult> ListarTipos()
        {
            // CORREGIDO: Llamada asíncrona correcta
            var tipos = await _activoNegocio.ListarTiposAsync();
            return Ok(tipos);
        }

        [HttpPost("tipos")]
        public async Task<IActionResult> GuardarTipo([FromBody] TipoActivo tipo)
        {
            try
            {
                await _activoNegocio.GuardarTipoAsync(tipo);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("tipos/{id}")]
        public async Task<IActionResult> EliminarTipo(int id)
        {
            try
            {
                await _activoNegocio.EliminarTipoAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}