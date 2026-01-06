using ITSM.Entidades;
using ITSM.Entidades.DTOs;
using ITSM.Negocio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;  // ⭐ AGREGAR ESTE USING

namespace ITSM.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TicketController : ControllerBase
    {
        private readonly TicketNegocio _ticketNegocio;

        public TicketController(TicketNegocio ticketNegocio)
        {
            _ticketNegocio = ticketNegocio;
        }

        // GET: api/ticket
        [HttpGet]
        public async Task<ActionResult<List<Ticket>>> Listar()
        {
            var lista = await _ticketNegocio.ListarTicketsAsync();
            return Ok(lista);
        }

        // GET: api/ticket/mis/{idUsuario}
        [HttpGet("mis/{idUsuario}")]
        public async Task<ActionResult<List<Ticket>>> ListarMisTickets(int idUsuario)
        {
            var lista = await _ticketNegocio.ListarMisTicketsAsync(idUsuario);
            return Ok(lista);
        }

        // GET: api/ticket/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> Obtener(int id)
        {
            var ticket = await _ticketNegocio.ObtenerTicketPorIdAsync(id);
            if (ticket == null) return NotFound();
            return Ok(ticket);
        }

        // ⭐⭐⭐ MODIFICADO: Extraer IdUsuario del JWT
        [HttpPost]
        public async Task<ActionResult<Ticket>> Registrar([FromBody] Ticket ticket)
        {
            try
            {
                // ✅ EXTRAER ID DEL USUARIO AUTENTICADO DESDE EL JWT
                var idUsuarioClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                                     ?? User.FindFirst("IdUsuario")?.Value;

                if (string.IsNullOrEmpty(idUsuarioClaim) || !int.TryParse(idUsuarioClaim, out int idUsuario))
                {
                    Console.WriteLine("❌ No se pudo obtener IdUsuario del JWT");
                    return Unauthorized(new { exito = false, mensaje = "No se pudo identificar al usuario autenticado" });
                }

                Console.WriteLine($"✅ Usuario autenticado: IdUsuario={idUsuario}");

                // ✅ ASIGNAR EL ID DEL SOLICITANTE DESDE EL JWT
                ticket.IdSolicitante = idUsuario;

                // Registrar el ticket
                var ticketCreado = await _ticketNegocio.RegistrarTicketAsync(ticket);

                return Ok(new
                {
                    exito = true,
                    mensaje = $"Ticket {ticketCreado.CodigoTicket} registrado exitosamente",
                    datos = ticketCreado
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al registrar ticket: {ex.Message}");
                return BadRequest(new
                {
                    exito = false,
                    mensaje = ex.Message
                });
            }
        }

        // PUT: api/ticket/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Ticket ticket)
        {
            if (id != ticket.IdTicket)
                return BadRequest("Id de ticket no coincide");

            try
            {
                await _ticketNegocio.ActualizarTicketAsync(ticket);
                return Ok(new { exito = true, mensaje = "Ticket actualizado" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { exito = false, mensaje = ex.Message });
            }
        }

        // POST: api/ticket/{id}/asignar/{idEspecialista}
        [HttpPost("{id}/asignar/{idEspecialista}")]
        public async Task<IActionResult> Asignar(int id, int idEspecialista)
        {
            try
            {
                await _ticketNegocio.AsignarTicketAsync(id, idEspecialista);
                return Ok(new { exito = true, mensaje = "Ticket asignado" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { exito = false, mensaje = ex.Message });
            }
        }

        // GET: api/ticket/categorias
        [HttpGet("categorias")]
        public async Task<ActionResult<List<Categoria>>> ListarCategorias()
        {
            var lista = await _ticketNegocio.ListarCategoriasAsync();
            return Ok(lista);
        }

        // GET: api/ticket/prioridades
        [HttpGet("prioridades")]
        public async Task<ActionResult<List<Prioridad>>> ListarPrioridades()
        {
            var lista = await _ticketNegocio.ListarPrioridadesAsync();
            return Ok(lista);
        }

        // GET: api/ticket/estados
        [HttpGet("estados")]
        public async Task<ActionResult<List<EstadoTicket>>> ListarEstados()
        {
            var lista = await _ticketNegocio.ListarEstadosAsync();
            return Ok(lista);
        }

        // GET: api/ticket/kpi
        [HttpGet("kpi")]
        public async Task<ActionResult<DashboardKpi>> ObtenerKpi()
        {
            var kpi = await _ticketNegocio.ObtenerKpisAsync();
            return Ok(kpi);
        }

        // GET: api/ticket/{id}/detalle
        [HttpGet("{id}/detalle")]
        public async Task<ActionResult<List<TicketComentario>>> ListarDetalles(int id)
        {
            var lista = await _ticketNegocio.ListarDetallesTicketAsync(id);
            return Ok(lista);
        }

        // POST: api/ticket/{id}/detalle
        [HttpPost("{id}/detalle")]
        public async Task<IActionResult> AgregarDetalle(int id, [FromBody] TicketComentario detalle)
        {
            if (detalle == null)
                return BadRequest("Detalle requerido");

            detalle.IdTicket = id;
            await _ticketNegocio.AgregarComentarioAsync(detalle);
            return Ok(new { exito = true, mensaje = "Comentario agregado" });
        }
    }
}
