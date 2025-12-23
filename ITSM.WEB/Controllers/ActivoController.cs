using Microsoft.AspNetCore.Mvc;
using ITSM.Negocio;
using ITSM.Entidades;

namespace ITSM.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivoController : ControllerBase
    {
        private readonly ActivoNegocio _activoNegocio;

        public ActivoController(ActivoNegocio activoNegocio)
        {
            _activoNegocio = activoNegocio;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var activos = await _activoNegocio.ListarActivos();
            return Ok(activos);
        }

        // ... resto de métodos (GetById, Post, Put, Tipos)
    }
}