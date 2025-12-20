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
            => Ok(await _ticketNegocio.ListarCategoriasAsync());

        [HttpGet("usuario/{id}")]
        public async Task<IActionResult> GetPorUsuario(int id)
            => Ok(await _ticketNegocio.ListarTicketsPorUsuarioAsync(id));

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Ticket ticket)
        {
            var resultado = await _ticketNegocio.GuardarTicketAsync(ticket);
            return Ok(resultado);
        }
    }
}