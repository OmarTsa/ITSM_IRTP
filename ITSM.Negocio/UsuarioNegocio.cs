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

        public async Task<Usuario?> ValidarLoginAsync(string username, string password)
        {
            // NOTA: Aquí deberías comparar hashes en un entorno real.
            // Para la tesis inicial, validamos texto plano si así lo tienes en DB,
            // pero idealmente usa BCrypt o SHA256.
            return await _contexto.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == password && u.Estado == 1);
        }

        public async Task<List<Usuario>> ListarTecnicosAsync()
        {
            // Asumiendo que el Rol 3 o 4 son técnicos/especialistas
            return await _contexto.Usuarios
                .Where(u => u.IdRol == 4 && u.Estado == 1)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            return await _contexto.Usuarios.FindAsync(id);
        }
    }
}