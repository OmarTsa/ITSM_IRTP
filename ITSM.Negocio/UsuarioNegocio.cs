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
                bool claveValida = BCrypt.Net.BCrypt.Verify(password, usuario.Password);
                if (claveValida) return usuario;
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