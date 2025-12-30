using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;

namespace ITSM.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilController : ControllerBase
    {
        // Endpoint para generar hash BCrypt
        [HttpGet("hashear/{password}")]
        public IActionResult HashearPassword(string password)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(password);
            return Ok(new
            {
                passwordOriginal = password,
                hashBCrypt = hash,
                longitud = hash.Length
            });
        }
    }
}
