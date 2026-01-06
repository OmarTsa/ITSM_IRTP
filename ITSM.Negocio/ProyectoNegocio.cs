using Microsoft.EntityFrameworkCore;
using ITSM.Datos;
using ITSM.Entidades;

namespace ITSM.Negocio
{
    public class ProyectoNegocio
    {
        private readonly ContextoBD _context;

        public ProyectoNegocio(ContextoBD context)
        {
            _context = context;
        }

        // ===================================================================
        // GESTIÓN DE PROYECTOS
        // ===================================================================

        public async Task<List<Proyecto>> ListarProyectosAsync()
        {
            return await _context.Proyectos
                .Include(p => p.Responsable)
                .Include(p => p.Area)
                .Where(p => p.Eliminado == 0)
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();
        }

        public async Task<Proyecto?> ObtenerProyectoPorIdAsync(int idProyecto)
        {
            return await _context.Proyectos
                .Include(p => p.Responsable)
                .Include(p => p.Area)
                .FirstOrDefaultAsync(p => p.IdProyecto == idProyecto && p.Eliminado == 0);
        }

        public async Task<Proyecto> RegistrarProyectoAsync(Proyecto proyecto)
        {
            proyecto.FechaCreacion = DateTime.Now;
            proyecto.Estado ??= "PLANIFICACION";
            proyecto.Prioridad ??= "MEDIA";
            proyecto.Eliminado = 0;

            _context.Proyectos.Add(proyecto);
            await _context.SaveChangesAsync();

            return await ObtenerProyectoPorIdAsync(proyecto.IdProyecto) ?? proyecto;
        }

        public async Task ActualizarProyectoAsync(Proyecto proyecto)
        {
            var proyectoDb = await _context.Proyectos.FindAsync(proyecto.IdProyecto);
            if (proyectoDb == null) throw new Exception("Proyecto no encontrado");

            proyectoDb.Nombre = proyecto.Nombre;
            proyectoDb.Descripcion = proyecto.Descripcion;
            proyectoDb.TipoProyecto = proyecto.TipoProyecto;
            proyectoDb.FechaInicio = proyecto.FechaInicio;
            proyectoDb.FechaFinPrevista = proyecto.FechaFinPrevista;
            proyectoDb.FechaFinReal = proyecto.FechaFinReal;
            proyectoDb.PresupuestoAsignado = proyecto.PresupuestoAsignado;
            proyectoDb.PresupuestoEjecutado = proyecto.PresupuestoEjecutado;
            proyectoDb.Estado = proyecto.Estado;
            proyectoDb.Prioridad = proyecto.Prioridad;
            proyectoDb.IdResponsable = proyecto.IdResponsable;
            proyectoDb.IdArea = proyecto.IdArea;
            proyectoDb.PorcentajeAvance = proyecto.PorcentajeAvance;
            proyectoDb.EsEstrategico = proyecto.EsEstrategico;
            proyectoDb.AlineadoPgd = proyecto.AlineadoPgd;

            await _context.SaveChangesAsync();
        }

        public async Task EliminarProyectoAsync(int idProyecto)
        {
            var proyecto = await _context.Proyectos.FindAsync(idProyecto);
            if (proyecto != null)
            {
                proyecto.Eliminado = 1;
                await _context.SaveChangesAsync();
            }
        }

        // ===================================================================
        // GESTIÓN DE HITOS
        // ===================================================================

        public async Task<List<Hito>> ListarHitosPorProyectoAsync(int idProyecto)
        {
            return await _context.Hitos
                .Include(h => h.Responsable)
                .Where(h => h.IdProyecto == idProyecto)
                .OrderBy(h => h.Orden)
                .ThenBy(h => h.FechaPrevista)
                .ToListAsync();
        }

        public async Task RegistrarHitoAsync(Hito hito)
        {
            hito.Estado ??= "PENDIENTE";
            _context.Hitos.Add(hito);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarHitoAsync(Hito hito)
        {
            _context.Hitos.Update(hito);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarHitoAsync(int idHito)
        {
            var hito = await _context.Hitos.FindAsync(idHito);
            if (hito != null)
            {
                _context.Hitos.Remove(hito);
                await _context.SaveChangesAsync();
            }
        }

        // ===================================================================
        // GESTIÓN DE ENTREGABLES
        // ===================================================================

        public async Task<List<Entregable>> ListarEntregablesPorProyectoAsync(int idProyecto)
        {
            return await _context.Entregables
                .Include(e => e.Hito)
                .Include(e => e.Aprobador)
                .Where(e => e.IdProyecto == idProyecto)
                .OrderBy(e => e.FechaEntregaPrevista)
                .ToListAsync();
        }

        public async Task RegistrarEntregableAsync(Entregable entregable)
        {
            entregable.Estado ??= "PENDIENTE";
            entregable.Aprobado = 0;
            _context.Entregables.Add(entregable);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarEntregableAsync(Entregable entregable)
        {
            _context.Entregables.Update(entregable);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarEntregableAsync(int idEntregable)
        {
            var entregable = await _context.Entregables.FindAsync(idEntregable);
            if (entregable != null)
            {
                _context.Entregables.Remove(entregable);
                await _context.SaveChangesAsync();
            }
        }
    }
}
