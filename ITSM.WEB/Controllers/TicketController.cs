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
        public async Task<IActionResult> GetCategorias()
        {
            var lista = await _ticketNegocio.ListarCategoriasAsync();
            return Ok(lista);
        }

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
            var resultado = await _ticketNegocio.GuardarTicketAsync(ticket);
            return Ok(resultado);
        }

        [HttpPost("cambiar-estado")]
        public async Task<IActionResult> CambiarEstado([FromBody] CambioEstadoRequest request)
        {
            await _ticketNegocio.CambiarEstadoTicketAsync(request.IdTicket, request.NuevoEstado, request.IdUsuario, request.Notas);
            return Ok();
        }
    }

    public class CambioEstadoRequest
    {
        public int IdTicket { get; set; }
        public int NuevoEstado { get; set; }
        public int IdUsuario { get; set; }
        public string? Notas { get; set; }
    }
}