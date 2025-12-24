using Microsoft.AspNetCore.Mvc;
using ITSM.Negocio;
using ITSM.Entidades;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            // CORREGIDO: Coincide con TicketNegocio.cs
            var lista = await _ticketNegocio.ListarTicketsAsync();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var ticket = await _ticketNegocio.ObtenerTicketPorIdAsync(id);
            if (ticket == null) return NotFound();
            return Ok(ticket);
        }

        [HttpGet("mis-tickets")]
        public async Task<IActionResult> MisTickets()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            int idUsuario = int.Parse(userIdClaim.Value);

            // CORREGIDO: Coincide con TicketNegocio.cs
            var lista = await _ticketNegocio.ListarMisTicketsAsync(idUsuario);
            return Ok(lista);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Ticket ticket)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    ticket.IdSolicitante = int.Parse(userIdClaim.Value);
                }

                // CORREGIDO: Coincide con TicketNegocio.cs
                await _ticketNegocio.RegistrarTicketAsync(ticket);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("comentario")]
        public async Task<IActionResult> AgregarComentario([FromBody] TicketDetalle detalle)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null) detalle.IdUsuario = int.Parse(userIdClaim.Value);

                await _ticketNegocio.AgregarComentarioAsync(detalle);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("categorias")]
        public async Task<IActionResult> ListarCategorias()
        {
            var lista = await _ticketNegocio.ListarCategoriasAsync();
            return Ok(lista);
        }

        [HttpGet("prioridades")]
        public async Task<IActionResult> ListarPrioridades()
        {
            var lista = await _ticketNegocio.ListarPrioridadesAsync();
            return Ok(lista);
        }

        [HttpGet("kpis")]
        public async Task<IActionResult> ObtenerKpis()
        {
            // Ahora funcionará porque arreglamos el DTO en el paso 1
            var kpis = await _ticketNegocio.ObtenerKpisAsync();
            return Ok(kpis);
        }
    }
}