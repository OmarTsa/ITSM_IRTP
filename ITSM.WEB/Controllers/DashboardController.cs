using ITSM.Entidades.DTOs;
using ITSM.Negocio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITSM.WEB.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly TicketNegocio _ticketNegocio;

    public DashboardController(TicketNegocio ticketNegocio)
    {
        _ticketNegocio = ticketNegocio;
    }

    [HttpGet("kpi")]
    public async Task<ActionResult<DashboardKpi>> ObtenerKpi()
    {
        try
        {
            var kpi = await _ticketNegocio.ObtenerKpisAsync();
            return Ok(kpi);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error al obtener KPI", error = ex.Message });
        }
    }
}

