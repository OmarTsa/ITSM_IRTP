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
            var lista = await _ticketNegocio.ListarTicketsAsync();
            return Ok(lista);
        }

        // NUEVO ENDPOINT: Mis Tickets
        [HttpGet("usuario/{idUsuario}")]
        public async Task<IActionResult> GetPorUsuario(int idUsuario)
        {
            var lista = await _ticketNegocio.ListarTicketsPorUsuarioAsync(idUsuario);
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _ticketNegocio.ObtenerTicketPorIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Ticket ticket)
        {
            if (ticket == null) return BadRequest();
            var resultado = await _ticketNegocio.GuardarTicketAsync(ticket);
            return Ok(resultado);
        }

        // --- Endpoints para Listas Desplegables ---

        [HttpGet("categorias")]
        public async Task<IActionResult> GetCategorias()
        {
            return Ok(await _ticketNegocio.ListarCategoriasAsync());
        }

        [HttpGet("prioridades")]
        public async Task<IActionResult> GetPrioridades()
        {
            return Ok(await _ticketNegocio.ListarPrioridadesAsync());
        }

        [HttpGet("estados")]
        public async Task<IActionResult> GetEstados()
        {
            return Ok(await _ticketNegocio.ListarEstadosAsync());
        }
    }
}