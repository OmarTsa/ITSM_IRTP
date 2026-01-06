using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ITSM.Negocio;
using ITSM.Entidades;

namespace ITSM.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // ?? SEGURIDAD: Todas las rutas requieren autenticación
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioNegocio _usuarioNegocio;

        public UsuarioController(UsuarioNegocio usuarioNegocio)
        {
            _usuarioNegocio = usuarioNegocio;
        }

        /// <summary>
        /// Obtener todos los usuarios - SOLO ADMINISTRADOR
        /// ?? SEGURIDAD: Protegido por rol ADMINISTRADOR
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<ActionResult<List<Usuario>>> ObtenerTodos()
        {
            try
            {
                var rol = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
                Console.WriteLine($"?? GET /api/usuario - Rol: {rol}");
                
                var usuarios = await _usuarioNegocio.ListarUsuariosAsync();
                
                // ?? SEGURIDAD: No retornar contraseñas hasheadas
                foreach (var u in usuarios)
                {
                    u.PasswordHash = null;
                }
                
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Error: {ex.Message}");
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtener usuario por ID
        /// ?? SEGURIDAD: Solo admin o el mismo usuario puede ver sus datos
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> ObtenerPorId(int id)
        {
            try
            {
                var usuarioActual = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var rolActual = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
                
                // ?? SEGURIDAD: Solo admin o el mismo usuario
                if (rolActual != "ADMINISTRADOR" && usuarioActual != id.ToString())
                {
                    Console.WriteLine($"? Acceso denegado: Usuario {usuarioActual} intentó acceder a {id}");
                    return Forbid(); // 403 Forbidden
                }

                var usuario = await _usuarioNegocio.ObtenerPorIdAsync(id);
                if (usuario == null)
                    return NotFound(new { mensaje = "Usuario no encontrado" });

                // ?? SEGURIDAD: No retornar contraseña
                usuario.PasswordHash = null;
                
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Error: {ex.Message}");
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Crear nuevo usuario - SOLO ADMINISTRADOR
        /// ?? SEGURIDAD: Validación de datos y rol ADMINISTRADOR
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<ActionResult<Usuario>> Crear([FromBody] dynamic modelo)
        {
            try
            {
                // ?? SEGURIDAD: Validación de entrada
                string username = modelo.username?.ToString();
                string password = modelo.password?.ToString();
                
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    return BadRequest(new { mensaje = "Username y password son obligatorios" });
                }

                // ?? SEGURIDAD: Validar unicidad antes de crear
                if (await _usuarioNegocio.ExisteUsernameAsync(username))
                {
                    return BadRequest(new { mensaje = "El username ya existe" });
                }

                var usuario = new Usuario
                {
                    Username = username,
                    Nombres = modelo.nombres?.ToString() ?? "",
                    Apellidos = modelo.apellidos?.ToString() ?? "",
                    Dni = modelo.dni?.ToString() ?? "",
                    Correo = modelo.correo?.ToString() ?? "",
                    IdRol = Convert.ToInt32(modelo.idRol),
                    IdArea = Convert.ToInt32(modelo.idArea),
                    Cargo = modelo.cargo?.ToString() ?? "",
                    Estado = 1 // Activo por defecto
                };

                await _usuarioNegocio.RegistrarUsuarioAsync(usuario, password);
                
                Console.WriteLine($"? Usuario creado: {usuario.Username} (ID: {usuario.IdUsuario})");
                
                // ?? SEGURIDAD: No retornar contraseña en respuesta
                usuario.PasswordHash = null;
                
                return CreatedAtAction(nameof(ObtenerPorId), new { id = usuario.IdUsuario }, usuario);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Error al crear usuario: {ex.Message}");
                return BadRequest(new { mensaje = "Error al crear usuario", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Actualizar usuario - SOLO ADMINISTRADOR
        /// ?? SEGURIDAD: Validación de ID y rol ADMINISTRADOR
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<ActionResult> Actualizar(int id, [FromBody] Usuario usuario)
        {
            try
            {
                // ?? SEGURIDAD: Validación de ID
                if (id != usuario.IdUsuario)
                {
                    return BadRequest(new { mensaje = "ID no coincide" });
                }

                await _usuarioNegocio.ActualizarUsuarioAsync(usuario);
                
                Console.WriteLine($"? Usuario actualizado: ID {id}");
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Error al actualizar: {ex.Message}");
                return BadRequest(new { mensaje = "Error al actualizar usuario", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Cambiar contraseña
        /// ?? SEGURIDAD: Usuario solo puede cambiar su propia contraseña (o admin puede cambiar cualquiera)
        /// </summary>
        [HttpPut("{id}/cambiar-password")]
        public async Task<ActionResult> CambiarPassword(int id, [FromBody] dynamic modelo)
        {
            try
            {
                var usuarioActual = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var rolActual = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
                
                // ?? SEGURIDAD: Solo admin o el mismo usuario
                if (rolActual != "ADMINISTRADOR" && usuarioActual != id.ToString())
                {
                    Console.WriteLine($"? Intento no autorizado de cambio de password: Usuario {usuarioActual} ? Usuario {id}");
                    return Forbid();
                }

                string nuevaPassword = modelo.password?.ToString();
                
                if (string.IsNullOrWhiteSpace(nuevaPassword))
                {
                    return BadRequest(new { mensaje = "La contraseña no puede estar vacía" });
                }

                await _usuarioNegocio.CambiarContrasenaAsync(id, nuevaPassword);
                
                Console.WriteLine($"? Contraseña cambiada para usuario ID: {id}");
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Error al cambiar contraseña: {ex.Message}");
                return BadRequest(new { mensaje = "Error al cambiar contraseña" });
            }
        }

        /// <summary>
        /// Dar de baja usuario (soft delete) - SOLO ADMINISTRADOR
        /// ?? SEGURIDAD: No eliminación física, solo lógica
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<ActionResult> DarDeBaja(int id)
        {
            try
            {
                await _usuarioNegocio.DarDeBajaAsync(id);
                
                Console.WriteLine($"? Usuario dado de baja: ID {id}");
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Error al dar de baja: {ex.Message}");
                return BadRequest(new { mensaje = "Error al dar de baja usuario", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Verificar si un username existe
        /// ?? SEGURIDAD: Solo ADMIN puede verificar
        /// </summary>
        [HttpGet("existe-username/{username}")]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<ActionResult<bool>> ExisteUsername(string username)
        {
            try
            {
                var existe = await _usuarioNegocio.ExisteUsernameAsync(username);
                return Ok(new { existe });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Error: {ex.Message}");
                return StatusCode(500, new { mensaje = "Error interno" });
            }
        }

        /// <summary>
        /// Listar roles activos
        /// </summary>
        [HttpGet("roles")]
        public async Task<ActionResult<List<Rol>>> ListarRoles()
        {
            try
            {
                var roles = await _usuarioNegocio.ListarRolesActivosAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Error: {ex.Message}");
                return StatusCode(500, new { mensaje = "Error interno" });
            }
        }

        /// <summary>
        /// Listar áreas
        /// </summary>
        [HttpGet("areas")]
        public async Task<ActionResult<List<Area>>> ListarAreas()
        {
            try
            {
                var areas = await _usuarioNegocio.ListarAreasAsync();
                return Ok(areas);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Error: {ex.Message}");
                return StatusCode(500, new { mensaje = "Error interno" });
            }
        }
    }
}
