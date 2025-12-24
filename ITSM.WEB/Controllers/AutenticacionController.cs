using Microsoft.AspNetCore.Mvc;
using ITSM.Negocio;
using ITSM.Entidades; // Importante para 'Usuario'
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
            try
            {
                // La validación de contraseña segura (BCrypt) ocurre dentro de este método
                var usuario = await _usuarioNegocio.LoginAsync(login.Username, login.Password);

                if (usuario == null)
                    return Unauthorized(new { mensaje = "Credenciales incorrectas o usuario inactivo." });

                // Generar el Token JWT
                var keyString = _config["Jwt:Key"] ?? "ClaveSecretaSuperSeguraParaTuTesis2025IRTP";
                var keyBytes = Encoding.UTF8.GetBytes(keyString);

                var claims = new ClaimsIdentity();
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()));
                claims.AddClaim(new Claim(ClaimTypes.Name, usuario.Username));
                claims.AddClaim(new Claim(ClaimTypes.Role, usuario.Rol?.Nombre ?? "Usuario"));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddHours(8), // Duración de la sesión laboral
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);
                var tokenCreado = tokenHandler.WriteToken(tokenConfig);

                // Responder con el DTO (sin incluir datos sensibles)
                var sesionDto = new SesionDto
                {
                    Nombre = usuario.NombreCompleto,
                    Username = usuario.Username,
                    Rol = usuario.Rol?.Nombre ?? "Sin Rol",
                    Token = tokenCreado
                };

                return Ok(sesionDto);
            }
            catch (Exception ex)
            {
                // Devolvemos el error detallado para diagnosticar problemas como ORA-00904
                return BadRequest(new { mensaje = "Error interno en el servidor", detalle = ex.Message });
            }
        }

        // =============================================================================
        // HERRAMIENTAS DE DESARROLLO (Ejecutar desde el navegador)
        // =============================================================================

        // URL: https://localhost:TU_PUERTO/api/autenticacion/crear-admin
        [HttpGet("crear-admin")]
        public async Task<IActionResult> CrearUsuarioAdmin()
        {
            try
            {
                var username = "otito";
                var password = "admin123";

                // 1. Verificar si ya existe
                var existente = await _usuarioNegocio.ObtenerPorUsernameAsync(username);

                if (existente != null)
                {
                    // Si existe, le actualizamos la contraseña para que funcione con BCrypt
                    await _usuarioNegocio.CambiarContrasenaAsync(existente.IdUsuario, password);
                    return Ok($"Usuario '{username}' encontrado. Contraseña actualizada exitosamente a '{password}'. ¡Intenta loguearte!");
                }
                else
                {
                    // Si no existe, lo creamos desde cero
                    var nuevo = new Usuario
                    {
                        Username = username,
                        Nombres = "OTITO",
                        Apellidos = "ADMINISTRADOR",
                        Dni = "99999999", // DNI Genérico
                        Correo = "otito@admin.com",
                        IdRol = 1, // Asumiendo que 1 es Admin
                        IdArea = 1, // Asumiendo que 1 es un área válida
                        Cargo = "Jefe TI"
                    };

                    await _usuarioNegocio.RegistrarUsuarioAsync(nuevo, password);
                    return Ok($"Usuario '{username}' creado exitosamente con contraseña '{password}'.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al procesar usuario: {ex.Message}");
            }
        }

        // URL: https://localhost:TU_PUERTO/api/autenticacion/reparar-db
        [HttpGet("reparar-db")]
        public async Task<IActionResult> RepararBaseDeDatos()
        {
            try
            {
                var resultado = await _usuarioNegocio.AutoRepararTablaUsuariosAsync();
                return Ok(new { mensaje = "Proceso de reparación finalizado", log = resultado });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error crítico al reparar BD: {ex.Message}");
            }
        }
    }
}