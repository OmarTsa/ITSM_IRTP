using Microsoft.EntityFrameworkCore;
using ITSM.Datos;
using ITSM.Entidades;

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
            // Login usando Email (CORREO en BD)
            // Si quieres login por Username, cambia u.Email por u.Username
            return await _contexto.Usuarios
                .Include(u => u.Rol)
                .Where(u => u.Email == email && u.Password == password && u.Activo == 1)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Usuario>> ListarUsuarios()
        {
            return await _contexto.Usuarios
                .Include(u => u.Rol)
                .Where(u => u.Activo == 1)
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
                usuario.Activo = 1;
                _contexto.Usuarios.Add(usuario);
            }
            else
            {
                _contexto.Usuarios.Update(usuario);
            }
            await _contexto.SaveChangesAsync();
        }
    }
}