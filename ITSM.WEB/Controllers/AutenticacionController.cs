using Microsoft.AspNetCore.Mvc;
using ITSM.Negocio;
using ITSM.Entidades;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace ITSM.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        // 1. Declaramos las variables privadas (Negocio y Configuración)
        private readonly UsuarioNegocio _usuarioNegocio;
        private readonly IConfiguration _config;

        // 2. INYECCIÓN DE DEPENDENCIAS (CONSTRUCTOR)
        // Aquí es donde "inyectamos" lo que antes intentabas hacer con @inject
        public AutenticacionController(UsuarioNegocio usuarioNegocio, IConfiguration config)
        {
            _usuarioNegocio = usuarioNegocio;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Usamos la capa de Negocio para buscar al usuario
            var usuario = await _usuarioNegocio.Login(request.Email, request.Password);

            if (usuario == null)
            {
                return Unauthorized(new { mensaje = "Credenciales incorrectas o usuario inactivo" });
            }

            // 3. CREACIÓN DE CLAIMS (DATOS DEL TOKEN)
            var claims = new List<Claim>
            {
                // .ToString() es obligatorio aquí
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreCompleto),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol?.Nombre ?? "Usuario"),
                new Claim("Username", usuario.Username)
            };

            // 4. GENERACIÓN DE LA LLAVE DE SEGURIDAD
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? "ClaveSecretaSuperSegura123!"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 5. CREACIÓN DEL TOKEN JWT
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: creds
            );

            // 6. RETORNO DE RESPUESTA EXITOSA
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                usuario = usuario
            });
        }

        // Clase auxiliar para recibir el JSON del login
        public class LoginRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
    }
}