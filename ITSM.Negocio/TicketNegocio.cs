using Microsoft.EntityFrameworkCore;
using ITSM.Datos;
using ITSM.Entidades;

namespace ITSM.Negocio
{
    public class TicketNegocio
    {
        private readonly ContextoBD _contexto;

        public TicketNegocio(ContextoBD contexto)
        {
            _contexto = contexto;
        }

        // --- MÉTODO AGREGADO PARA CORREGIR ERROR ---
        public async Task<List<Activo>> ListarActivosAsync()
        {
            return await _contexto.Activos
                .AsNoTracking()
                .ToListAsync();
        }

        // Métodos existentes conservados
        public async Task<List<Categoria>> ListarCategoriasAsync()
        {
            return await _contexto.Categorias.Where(c => c.Activo == 1).AsNoTracking().ToListAsync();
        }

        public async Task<List<Prioridad>> ListarPrioridadesAsync()
        {
            return await _contexto.Prioridades.AsNoTracking().ToListAsync();
        }

        public async Task<List<EstadoTicket>> ListarEstadosAsync()
        {
            return await _contexto.EstadosTicket.AsNoTracking().ToListAsync();
        }

        public async Task<List<Ticket>> ListarTicketsPorUsuarioAsync(int idUsuario)
        {
            return await _contexto.Tickets
                .Include(t => t.Estado)
                .Where(t => t.IdSolicitante == idUsuario)
                .OrderByDescending(t => t.FechaCreacion)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Ticket>> ListarTodosLosTicketsAsync()
        {
            return await _contexto.Tickets
                .Include(t => t.Estado)
                .Include(t => t.Categoria)
                .Include(t => t.Solicitante)
                .OrderByDescending(t => t.FechaCreacion)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> GuardarTicketAsync(Ticket ticket)
        {
            if (ticket.FechaCreacion == default) ticket.FechaCreacion = DateTime.Now;
            _contexto.Tickets.Add(ticket);
            return await _contexto.SaveChangesAsync() > 0;
        }

        public async Task<Ticket?> ObtenerTicketPorIdAsync(int idTicket)
        {
            return await _contexto.Tickets
                .Include(t => t.Estado)
                .Include(t => t.Categoria)
                .Include(t => t.ActivoRelacionado)
                .FirstOrDefaultAsync(t => t.IdTicket == idTicket);
        }

        public async Task<List<Ticket>> GenerarReporteTicketsAsync(DateTime inicio, DateTime fin)
        {
            return await _contexto.Tickets
                .Include(t => t.Estado)
                .Where(t => t.FechaCreacion >= inicio && t.FechaCreacion <= fin)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}