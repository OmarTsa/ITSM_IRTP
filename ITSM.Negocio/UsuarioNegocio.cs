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

        // Método existente
        public async Task<Usuario?> ValidarLoginAsync(string username, string password)
        {
            return await _contexto.Usuarios
                .Include(u => u.Rol) // Asegúrate de tener la entidad Rol o quitar esto si no la usas
                .FirstOrDefaultAsync(u => u.NombreUsuario == username && u.Clave == password && u.Estado == 1);
        }

        // --- MÉTODOS AGREGADOS PARA CORREGIR ERRORES ---

        public async Task<Usuario?> LoginAsync(string username, string password)
        {
            // Reutilizamos la lógica de validación existente
            return await ValidarLoginAsync(username, password);
        }

        public async Task<List<Usuario>> ListarUsuariosAsync()
        {
            return await _contexto.Usuarios
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Usuario>> ListarTecnicosAsync()
        {
            // Asumiendo que IdRol 4 es técnico
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