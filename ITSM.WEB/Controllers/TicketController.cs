using ITSM.Entidades;
using ITSM.Entidades.DTOs;
using ITSM.Negocio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        // POST: api/ticket
        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] Ticket ticket)
        {
            await _ticketNegocio.RegistrarTicketAsync(ticket);
            return Ok();
        }

        // PUT: api/ticket/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Ticket ticket)
        {
            if (id != ticket.IdTicket)
                return BadRequest("Id de ticket no coincide");

            await _ticketNegocio.ActualizarTicketAsync(ticket);
            return Ok();
        }

        // POST: api/ticket/{id}/asignar/{idEspecialista}
        [HttpPost("{id}/asignar/{idEspecialista}")]
        public async Task<IActionResult> Asignar(int id, int idEspecialista)
        {
            await _ticketNegocio.AsignarTicketAsync(id, idEspecialista);
            return Ok();
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
        public async Task<ActionResult<List<TicketDetalle>>> ListarDetalles(int id)
        {
            var lista = await _ticketNegocio.ListarDetallesTicketAsync(id);
            return Ok(lista);
        }

        // POST: api/ticket/{id}/detalle
        [HttpPost("{id}/detalle")]
        public async Task<IActionResult> AgregarDetalle(int id, [FromBody] TicketDetalle detalle)
        {
            if (detalle == null)
                return BadRequest("Detalle requerido");

            detalle.IdTicket = id;
            await _ticketNegocio.AgregarComentarioAsync(detalle);
            return Ok();
        }
    }
}
