using Microsoft.EntityFrameworkCore;
using ITSM.Datos;
using ITSM.Entidades;

namespace ITSM.Negocio
{
    public class ActivoNegocio
    {
        private readonly ContextoBD _context;

        public ActivoNegocio(ContextoBD context)
        {
            _context = context;
        }

        // --- GESTIÓN DE ACTIVOS ---

        public async Task<List<Activo>> ListarActivosAsync()
        {
            return await _context.Activos
                .Include(a => a.TipoActivo)
                .Include(a => a.Estado)
                .Include(a => a.UsuarioAsignado)
                .Where(a => a.Eliminado == 0)
                .ToListAsync();
        }

        // IMPLEMENTADO: Requerido por ActivoController
        public async Task<List<Activo>> ListarActivosPorUsuarioAsync(int idUsuario)
        {
            return await _context.Activos
                .Include(a => a.TipoActivo)
                .Include(a => a.Estado)
                .Where(a => a.IdUsuarioAsignado == idUsuario && a.Eliminado == 0)
                .ToListAsync();
        }

        public async Task<Activo?> ObtenerPorIdAsync(int id)
        {
            return await _context.Activos
                .Include(a => a.TipoActivo)
                .Include(a => a.Estado)
                .Include(a => a.UsuarioAsignado)
                .FirstOrDefaultAsync(a => a.IdActivo == id);
        }

        public async Task GuardarActivoAsync(Activo activo)
        {
            if (activo.IdActivo == 0)
            {
                activo.FechaRegistro = DateTime.Now;
                activo.Eliminado = 0;
                _context.Activos.Add(activo);
            }
            else
            {
                _context.Activos.Update(activo);
            }
            await _context.SaveChangesAsync();
        }

        public async Task EliminarActivoAsync(int id)
        {
            var activo = await _context.Activos.FindAsync(id);
            if (activo != null)
            {
                activo.Eliminado = 1;
                await _context.SaveChangesAsync();
            }
        }

        // --- GESTIÓN DE TIPOS DE ACTIVO ---

        public async Task<List<TipoActivo>> ListarTiposAsync()
        {
            return await _context.TiposActivo
                .Where(t => t.Estado == 1)
                .ToListAsync();
        }

        // IMPLEMENTADO: Requerido por ActivoController (GestiónTipos.razor)
        public async Task GuardarTipoAsync(TipoActivo tipo)
        {
            if (tipo.IdTipo == 0)
            {
                tipo.Estado = 1;
                _context.TiposActivo.Add(tipo);
            }
            else
            {
                _context.TiposActivo.Update(tipo);
            }
            await _context.SaveChangesAsync();
        }

        // IMPLEMENTADO: Requerido por ActivoController
        public async Task EliminarTipoAsync(int id)
        {
            var tipo = await _context.TiposActivo.FindAsync(id);
            if (tipo != null)
            {
                tipo.Estado = 0; // Soft delete
                await _context.SaveChangesAsync();
            }
        }
    }
}
