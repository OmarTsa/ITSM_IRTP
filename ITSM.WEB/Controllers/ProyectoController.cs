using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ITSM.Entidades;
using ITSM.Negocio;

namespace ITSM.WEB.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProyectoController : ControllerBase
    {
        private readonly ProyectoNegocio _proyectoNegocio;

        public ProyectoController(ProyectoNegocio proyectoNegocio)
        {
            _proyectoNegocio = proyectoNegocio;
        }

        // ===================================================================
        // ENDPOINTS DE PROYECTOS
        // ===================================================================

        [HttpGet]
        public async Task<IActionResult> ListarProyectos()
        {
            try
            {
                var proyectos = await _proyectoNegocio.ListarProyectosAsync();
                return Ok(proyectos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error al listar proyectos: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerProyecto(int id)
        {
            try
            {
                var proyecto = await _proyectoNegocio.ObtenerProyectoPorIdAsync(id);
                if (proyecto == null)
                    return NotFound(new { mensaje = "Proyecto no encontrado" });

                return Ok(proyecto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error al obtener proyecto: {ex.Message}" });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Jefe")]
        public async Task<IActionResult> RegistrarProyecto([FromBody] Proyecto proyecto)
        {
            try
            {
                var proyectoCreado = await _proyectoNegocio.RegistrarProyectoAsync(proyecto);
                return CreatedAtAction(nameof(ObtenerProyecto), new { id = proyectoCreado.IdProyecto }, proyectoCreado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = $"Error al registrar proyecto: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador,Jefe")]
        public async Task<IActionResult> ActualizarProyecto(int id, [FromBody] Proyecto proyecto)
        {
            try
            {
                if (id != proyecto.IdProyecto)
                    return BadRequest(new { mensaje = "El ID del proyecto no coincide" });

                await _proyectoNegocio.ActualizarProyectoAsync(proyecto);
                return Ok(new { mensaje = "Proyecto actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = $"Error al actualizar proyecto: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EliminarProyecto(int id)
        {
            try
            {
                await _proyectoNegocio.EliminarProyectoAsync(id);
                return Ok(new { mensaje = "Proyecto eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = $"Error al eliminar proyecto: {ex.Message}" });
            }
        }

        // ===================================================================
        // ENDPOINTS DE HITOS
        // ===================================================================

        [HttpGet("{idProyecto}/hitos")]
        public async Task<IActionResult> ListarHitos(int idProyecto)
        {
            try
            {
                var hitos = await _proyectoNegocio.ListarHitosPorProyectoAsync(idProyecto);
                return Ok(hitos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error al listar hitos: {ex.Message}" });
            }
        }

        [HttpPost("hitos")]
        [Authorize(Roles = "Administrador,Jefe")]
        public async Task<IActionResult> RegistrarHito([FromBody] Hito hito)
        {
            try
            {
                await _proyectoNegocio.RegistrarHitoAsync(hito);
                return Ok(new { mensaje = "Hito registrado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = $"Error al registrar hito: {ex.Message}" });
            }
        }

        [HttpPut("hitos/{id}")]
        [Authorize(Roles = "Administrador,Jefe")]
        public async Task<IActionResult> ActualizarHito(int id, [FromBody] Hito hito)
        {
            try
            {
                if (id != hito.IdHito)
                    return BadRequest(new { mensaje = "El ID del hito no coincide" });

                await _proyectoNegocio.ActualizarHitoAsync(hito);
                return Ok(new { mensaje = "Hito actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = $"Error al actualizar hito: {ex.Message}" });
            }
        }

        [HttpDelete("hitos/{id}")]
        [Authorize(Roles = "Administrador,Jefe")]
        public async Task<IActionResult> EliminarHito(int id)
        {
            try
            {
                await _proyectoNegocio.EliminarHitoAsync(id);
                return Ok(new { mensaje = "Hito eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = $"Error al eliminar hito: {ex.Message}" });
            }
        }

        // ===================================================================
        // ENDPOINTS DE ENTREGABLES
        // ===================================================================

        [HttpGet("{idProyecto}/entregables")]
        public async Task<IActionResult> ListarEntregables(int idProyecto)
        {
            try
            {
                var entregables = await _proyectoNegocio.ListarEntregablesPorProyectoAsync(idProyecto);
                return Ok(entregables);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error al listar entregables: {ex.Message}" });
            }
        }

        [HttpPost("entregables")]
        [Authorize(Roles = "Administrador,Jefe")]
        public async Task<IActionResult> RegistrarEntregable([FromBody] Entregable entregable)
        {
            try
            {
                await _proyectoNegocio.RegistrarEntregableAsync(entregable);
                return Ok(new { mensaje = "Entregable registrado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = $"Error al registrar entregable: {ex.Message}" });
            }
        }

        [HttpPut("entregables/{id}")]
        [Authorize(Roles = "Administrador,Jefe")]
        public async Task<IActionResult> ActualizarEntregable(int id, [FromBody] Entregable entregable)
        {
            try
            {
                if (id != entregable.IdEntregable)
                    return BadRequest(new { mensaje = "El ID del entregable no coincide" });

                await _proyectoNegocio.ActualizarEntregableAsync(entregable);
                return Ok(new { mensaje = "Entregable actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = $"Error al actualizar entregable: {ex.Message}" });
            }
        }

        [HttpDelete("entregables/{id}")]
        [Authorize(Roles = "Administrador,Jefe")]
        public async Task<IActionResult> EliminarEntregable(int id)
        {
            try
            {
                await _proyectoNegocio.EliminarEntregableAsync(id);
                return Ok(new { mensaje = "Entregable eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = $"Error al eliminar entregable: {ex.Message}" });
            }
        }
    }
}
