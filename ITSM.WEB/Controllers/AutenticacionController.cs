using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ITSM.Negocio;
using ITSM.Entidades;
using ITSM.Entidades.DTOs;

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

        /// <summary>
        /// Login principal con JWT
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<SesionDto>> Login([FromBody] LoginDto modelo)
        {
            try
            {
                Console.WriteLine($"🔍 Login: Recibido username={modelo.Username}");

                // Validar credenciales
                var usuario = await _usuarioNegocio.LoginAsync(modelo.Username, modelo.Password);

                if (usuario == null)
                {
                    Console.WriteLine("❌ LoginAsync retornó NULL");
                    return Unauthorized(new { mensaje = "Credenciales incorrectas" });
                }

                Console.WriteLine($"✅ Usuario encontrado: {usuario.Username}, Rol: {usuario.Rol?.Nombre ?? "NULL"}");

                // Generar JWT
                var sesion = GenerarSesionJwt(usuario);

                Console.WriteLine($"✅ JWT generado exitosamente");
                return Ok(sesion);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Excepción en Login: {ex.Message}");
                return BadRequest(new { mensaje = "Error en login", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Genera el token JWT y retorna SesionDto
        /// </summary>
        private SesionDto GenerarSesionJwt(Usuario usuario)
        {
            // Crear Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, usuario.Username),
                new Claim(ClaimTypes.GivenName, $"{usuario.Nombres} {usuario.Apellidos}"),
                new Claim(ClaimTypes.Role, usuario.Rol?.Nombre ?? "Usuario"),
                new Claim("IdRol", usuario.IdRol.ToString())
            };

            // Configuración JWT
            var keyBytes = Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!);
            var llave = new SymmetricSecurityKey(keyBytes);
            var credenciales = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credenciales
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new SesionDto
            {
                IdUsuario = usuario.IdUsuario,  // ⭐ CORREGIDO: Agregar IdUsuario
                Nombre = $"{usuario.Nombres} {usuario.Apellidos}",
                Username = usuario.Username,
                Rol = usuario.Rol?.Nombre ?? string.Empty,
                Token = tokenString,
                
                // Propiedades adicionales para trazabilidad
                NombreCompleto = $"{usuario.Nombres} {usuario.Apellidos}",
                IdArea = usuario.IdArea,
                NombreArea = usuario.Area?.Nombre
            };
        }

        // =============================================================================
        // 🔧 HERRAMIENTAS DE DESARROLLO (MANTENER SOLO EN DEV)
        // =============================================================================

        /// <summary>
        /// Crea o actualiza el usuario admin
        /// URL: GET https://localhost:7244/api/autenticacion/crear-admin
        /// </summary>
        [HttpGet("crear-admin")]
        [AllowAnonymous]
        public async Task<IActionResult> CrearUsuarioAdmin()
        {
            try
            {
                var username = "otito";
                var password = "admin123";

                var existente = await _usuarioNegocio.ObtenerPorUsernameAsync(username);

                if (existente != null)
                {
                    await _usuarioNegocio.CambiarContrasenaAsync(existente.IdUsuario, password);
                    return Ok(new
                    {
                        success = true,
                        mensaje = $"✅ Usuario '{username}' actualizado",
                        credenciales = new { username, password }
                    });
                }
                else
                {
                    var nuevo = new Usuario
                    {
                        Username = username,
                        Nombres = "ADMIN",
                        Apellidos = "SISTEMA",
                        Dni = "00000000",
                        Correo = "admin@irtp.pe",
                        IdRol = 1,
                        IdArea = 1,
                        Cargo = "Administrador del Sistema"
                    };

                    await _usuarioNegocio.RegistrarUsuarioAsync(nuevo, password);
                    return Ok(new
                    {
                        success = true,
                        mensaje = $"✅ Usuario '{username}' creado",
                        credenciales = new { username, password }
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    mensaje = "❌ Error al procesar usuario",
                    detalle = ex.Message
                });
            }
        }

        /// <summary>
        /// Test de autenticación actual
        /// </summary>
        [HttpGet("test-auth")]
        [Authorize]
        public IActionResult TestAuth()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            var userName = User.Identity?.Name;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                autenticado = User.Identity?.IsAuthenticated ?? false,
                usuario = userName,
                rol = role,
                claims
            });
        }
    }
}


