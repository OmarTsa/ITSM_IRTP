using Microsoft.AspNetCore.Mvc;
using ITSM.Negocio;
using ITSM.Entidades;

namespace ITSM.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly TicketNegocio _ticketNegocio;

        public TicketController(TicketNegocio ticketNegocio)
        {
            _ticketNegocio = ticketNegocio;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var lista = await _ticketNegocio.ObtenerTickets();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var ticket = await _ticketNegocio.ObtenerTicketPorId(id);
            if (ticket == null) return NotFound();
            return Ok(ticket);
        }

        [HttpGet("usuario/{id}")]
        public async Task<IActionResult> GetPorUsuario(int id)
        {
            var lista = await _ticketNegocio.ObtenerTicketsPorUsuario(id);
            return Ok(lista);
        }

        [HttpGet("categorias")]
        public async Task<IActionResult> GetCategorias()
        {
            var lista = await _ticketNegocio.ListarCategorias();
            return Ok(lista);
        }

        // NUEVO: ENDPOINT DASHBOARD
        [HttpGet("kpis/{idUsuario}")]
        public async Task<IActionResult> GetKpis(int idUsuario)
        {
            var kpis = await _ticketNegocio.ObtenerKpisAsync(idUsuario);
            return Ok(kpis);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Ticket ticket)
        {
            await _ticketNegocio.GuardarTicket(ticket);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Ticket ticket)
        {
            if (id != ticket.IdTicket) return BadRequest();
            await _ticketNegocio.GuardarTicket(ticket);
            return Ok();
        }
    }
}