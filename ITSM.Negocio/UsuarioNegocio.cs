using Microsoft.EntityFrameworkCore;
using ITSM.Datos;
using ITSM.Entidades;
using BCrypt.Net;

namespace ITSM.Negocio
{
    public class UsuarioNegocio
    {
        private readonly ContextoBD _contexto;

        public UsuarioNegocio(ContextoBD contexto)
        {
            _contexto = contexto;
        }

        public async Task<Usuario?> Login(string email, string password)
        {
            // 1. Buscamos al usuario por Email o Username (solo usuarios activos)
            var usuario = await _contexto.Usuarios
                .Include(u => u.Rol)
                .Where(u => (u.Email == email || u.Username == email) && u.Activo == 1)
                .FirstOrDefaultAsync();

            // 2. Si el usuario existe, verificamos el hash de la contraseña
            if (usuario != null && !string.IsNullOrEmpty(usuario.Password))
            {
                try
                {
                    var stored = usuario.Password ?? string.Empty;

                    // Detect simple BCrypt hashes (start with $2a$, $2b$, $2y$, etc.)
                    if (stored.StartsWith("$2a$") || stored.StartsWith("$2b$") || stored.StartsWith("$2y$") || stored.StartsWith("$2x$") || stored.StartsWith("$2z$"))
                    {
                        bool claveValida = BCrypt.Net.BCrypt.Verify(password, stored);
                        if (claveValida) return usuario;
                    }
                    else
                    {
                        // Legacy plaintext password stored. Compare directly and migrate to hashed password.
                        if (stored == password)
                        {
                            // Re-hash and update stored password securely
                            var hashed = BCrypt.Net.BCrypt.HashPassword(password);
                            var usuarioExistente = await _contexto.Usuarios.FindAsync(usuario.IdUsuario);
                            if (usuarioExistente != null)
                            {
                                usuarioExistente.Password = hashed;
                                _contexto.Usuarios.Update(usuarioExistente);
                                await _contexto.SaveChangesAsync();

                                // Update object to reflect new hashed password
                                usuario.Password = hashed;
                            }

                            return usuario;
                        }
                    }
                }
                catch
                {
                    // If verification process fails for any reason, treat as authentication failure
                    return null;
                }
            }

            return null;
        }

        public async Task<List<Usuario>> ListarUsuarios()
        {
            return await _contexto.Usuarios
                .Include(u => u.Rol)
                .OrderBy(u => u.Apellidos)
                .ToListAsync();
        }

        public async Task<Usuario?> ObtenerPorId(int id)
        {
            return await _contexto.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.IdUsuario == id);
        }

        public async Task GuardarUsuario(Usuario usuario)
        {
            if (usuario.IdUsuario == 0)
            {
                // SEGURIDAD: Hashear contraseña antes de insertar en la BD
                if (!string.IsNullOrEmpty(usuario.Password))
                {
                    usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
                }

                if (usuario.IdRol == 0) usuario.IdRol = 5; // Default: USUARIO_FINAL
                _contexto.Usuarios.Add(usuario);
            }
            else
            {
                // Evitar que el Update sobreescriba el password con null si no se envió desde el formulario
                var usuarioExistente = await _contexto.Usuarios.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.IdUsuario == usuario.IdUsuario);

                if (string.IsNullOrEmpty(usuario.Password) && usuarioExistente != null)
                {
                    usuario.Password = usuarioExistente.Password;
                }
                else if (!string.IsNullOrEmpty(usuario.Password))
                {
                    usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
                }

                _contexto.Usuarios.Update(usuario);
            }
            await _contexto.SaveChangesAsync();
        }
    }
}