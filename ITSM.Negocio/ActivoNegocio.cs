using Microsoft.EntityFrameworkCore;
using ITSM.Datos;
using ITSM.Entidades;

namespace ITSM.Negocio
{
    public class ActivoNegocio
    {
        private readonly ContextoBD _contexto;

        public ActivoNegocio(ContextoBD contexto)
        {
            _contexto = contexto;
        }

        // --- GESTIÓN DE BIENES (ACTIVOS) ---
        public async Task<List<Activo>> ListarActivos()
        {
            return await _contexto.Activos
                .Include(a => a.Tipo)
                .Include(a => a.UsuarioAsignado)
                .OrderBy(a => a.IdActivo)
                .ToListAsync();
        }

        public async Task<Activo?> ObtenerPorId(int id)
        {
            return await _contexto.Activos
                .Include(a => a.Tipo)
                .FirstOrDefaultAsync(a => a.IdActivo == id);
        }

        public async Task GuardarActivo(Activo activo)
        {
            // Validaciones básicas
            activo.Marca = activo.Marca.ToUpper();
            activo.Modelo = activo.Modelo.ToUpper();
            activo.Serie = activo.Serie.ToUpper();

            if (activo.IdActivo == 0)
            {
                // Asignar fecha registro si fuera necesario, o dejar que la BD lo haga
                _contexto.Activos.Add(activo);
            }
            else
            {
                _contexto.Activos.Update(activo);
            }
            await _contexto.SaveChangesAsync();
        }

        // --- GESTIÓN DE TIPOS (CATÁLOGO DINÁMICO) ---
        public async Task<List<TipoActivo>> ListarTipos()
        {
            return await _contexto.TiposActivo.OrderBy(t => t.Nombre).ToListAsync();
        }

        public async Task GuardarTipo(TipoActivo tipo)
        {
            if (string.IsNullOrWhiteSpace(tipo.Nombre)) throw new Exception("El nombre es obligatorio");

            tipo.Nombre = tipo.Nombre.ToUpper().Trim();

            if (tipo.IdTipo == 0)
            {
                if (await _contexto.TiposActivo.AnyAsync(t => t.Nombre == tipo.Nombre))
                    throw new Exception("Ya existe un tipo con este nombre");

                _contexto.TiposActivo.Add(tipo);
            }
            else
            {
                _contexto.TiposActivo.Update(tipo);
            }
            await _contexto.SaveChangesAsync();
        }

        public async Task EliminarTipo(int id)
        {
            // Validamos que no tenga activos asignados antes de borrar
            bool tieneActivos = await _contexto.Activos.AnyAsync(a => a.IdTipo == id);
            if (tieneActivos) throw new Exception("No se puede eliminar: Hay activos registrados de este tipo.");

            var tipo = await _contexto.TiposActivo.FindAsync(id);
            if (tipo != null)
            {
                _contexto.TiposActivo.Remove(tipo);
                await _contexto.SaveChangesAsync();
            }
        }
    }
}