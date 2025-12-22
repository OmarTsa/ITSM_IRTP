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

        [HttpGet("categorias")]
        public async Task<ActionResult<List<Categoria>>> ListarCategorias()
        {
            return Ok(await _ticketNegocio.ListarCategoriasAsync());
        }

        [HttpPost]
        public async Task<ActionResult> CrearTicket([FromBody] Ticket ticket)
        {
            await _ticketNegocio.GuardarTicketAsync(ticket);
            return Ok();
        }

        // Endpoint para Mis Tickets
        [HttpGet("usuario/{idUsuario}")]
        public async Task<ActionResult<List<Ticket>>> ObtenerPorUsuario(int idUsuario)
        {
            return Ok(await _ticketNegocio.ListarTicketsPorUsuarioAsync(idUsuario));
        }

        // Endpoint para Detalle
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> ObtenerPorId(int id)
        {
            var ticket = await _ticketNegocio.ObtenerTicketPorIdAsync(id);
            if (ticket == null) return NotFound();
            return Ok(ticket);
        }
    }
}