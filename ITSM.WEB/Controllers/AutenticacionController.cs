using Microsoft.AspNetCore.Mvc;
using ITSM.Negocio;
using ITSM.Entidades.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ITSM.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        private readonly UsuarioNegocio _usuarioNegocio;
        private readonly IConfiguration _config;

        public AutenticacionController(UsuarioNegocio usuarioNegocio, IConfiguration config)
        {
            _usuarioNegocio = usuarioNegocio;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var usuario = await _usuarioNegocio.LoginAsync(login.Username, login.Password);

            if (usuario == null)
                return Unauthorized(new { mensaje = "Credenciales incorrectas o usuario inactivo." });

            // Generar el Token JWT
            var key = _config["Jwt:Key"] ?? "ClaveSecretaSuperSeguraParaTuTesis2025IRTP";
            var keyBytes = Encoding.ASCII.GetBytes(key);

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()));
            claims.AddClaim(new Claim(ClaimTypes.Name, usuario.Username));
            claims.AddClaim(new Claim(ClaimTypes.Role, usuario.Rol?.Nombre ?? "Usuario"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(8), // Duración de la sesión
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);
            var tokenCreado = tokenHandler.WriteToken(tokenConfig);

            // Responder con el DTO
            var sesionDto = new SesionDto
            {
                Nombre = usuario.NombreCompleto,
                Username = usuario.Username,
                Rol = usuario.Rol?.Nombre ?? "Sin Rol",
                Token = tokenCreado
            };

            return Ok(sesionDto);
        }
    }
}