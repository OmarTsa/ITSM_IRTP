using Microsoft.EntityFrameworkCore;
using ITSM.Datos;
using ITSM.Entidades;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace ITSM.Negocio
{
    public class TicketNegocio
    {
        private readonly ContextoBD _contexto;

        public TicketNegocio(ContextoBD contexto)
        {
            _contexto = contexto;
        }

        public async Task<List<Ticket>> ListarTodosLosTicketsAsync()
        {
            return await _contexto.Tickets
                .Include(t => t.Categoria).Include(t => t.Prioridad)
                .Include(t => t.Estado).Include(t => t.Solicitante)
                .OrderByDescending(t => t.FechaCreacion).ToListAsync();
        }

        public async Task<Ticket?> ObtenerTicketPorIdAsync(int id)
        {
            return await _contexto.Tickets
                .Include(t => t.Categoria).Include(t => t.Prioridad)
                .Include(t => t.Estado).Include(t => t.Solicitante)
                .Include(t => t.ActivoRelacionado)
                .FirstOrDefaultAsync(t => t.IdTicket == id);
        }

        public async Task CambiarEstadoTicketAsync(int idTicket, int nuevoEstado, int idUsuarioOperador, string? notas = null)
        {
            var ticket = await _contexto.Tickets.FindAsync(idTicket);
            if (ticket != null)
            {
                ticket.IdEstado = nuevoEstado;
                if (nuevoEstado == 4 || nuevoEstado == 5)
                {
                    ticket.FechaCierre = DateTime.Now;
                    ticket.NotasCierre = notas;
                }
                await _contexto.SaveChangesAsync();
            }
        }

        public async Task<Dictionary<string, int>> ObtenerTicketsPorEstadoAsync()
        {
            return await _contexto.Tickets.Include(t => t.Estado)
                .GroupBy(t => t.Estado!.Nombre)
                .Select(g => new { E = g.Key, C = g.Count() })
                .ToDictionaryAsync(x => x.E, x => x.C);
        }

        public async Task<Dictionary<string, int>> ObtenerTicketsPorPrioridadAsync()
        {
            return await _contexto.Tickets.Include(t => t.Prioridad)
                .GroupBy(t => t.Prioridad!.Nombre)
                .Select(g => new { P = g.Key, C = g.Count() })
                .ToDictionaryAsync(x => x.P, x => x.C);
        }

        public async Task<List<Ticket>> GenerarReporteTicketsAsync(DateTime? inicio, DateTime? fin, int? idEstado)
        {
            var query = _contexto.Tickets
                .Include(t => t.Categoria).Include(t => t.Prioridad)
                .Include(t => t.Estado).Include(t => t.Solicitante)
                .AsQueryable();

            if (inicio.HasValue) query = query.Where(t => t.FechaCreacion >= inicio.Value);
            if (fin.HasValue) query = query.Where(t => t.FechaCreacion <= fin.Value);
            if (idEstado.HasValue && idEstado > 0) query = query.Where(t => t.IdEstado == idEstado.Value);

            return await query.ToListAsync();
        }

        public async Task<List<Usuario>> ListarUsuariosAsync()
        {
            // CORRECCIÓN: Comparación numérica de int con int
            return await _contexto.Usuarios
                .Where(u => u.Activo == 1)
                .ToListAsync();
        }

        public async Task<bool> AsignarActivoAUsuarioAsync(int idActivo, int? idUsuario)
        {
            var activo = await _contexto.Activos.FindAsync(idActivo);
            if (activo == null) return false;
            activo.IdUsuarioAsignado = idUsuario;
            await _contexto.SaveChangesAsync();
            return true;
        }

        public async Task<List<EstadoTicket>> ListarEstadosAsync()
        {
            return await _contexto.Estados.ToListAsync();
        }
    }
}