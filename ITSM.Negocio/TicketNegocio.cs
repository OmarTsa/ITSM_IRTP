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

        public async Task<Dictionary<string, int>> ObtenerTicketsPorEstadoAsync()
        {
            return await _contexto.Tickets
                .Include(t => t.Estado)
                .GroupBy(t => t.Estado!.Nombre)
                .Select(g => new { Estado = g.Key, Cantidad = g.Count() })
                .ToDictionaryAsync(x => x.Estado, x => x.Cantidad);
        }

        public async Task<Dictionary<string, int>> ObtenerTicketsPorPrioridadAsync()
        {
            return await _contexto.Tickets
                .Include(t => t.Prioridad)
                .GroupBy(t => t.Prioridad!.Nombre)
                .Select(g => new { Prioridad = g.Key, Cantidad = g.Count() })
                .ToDictionaryAsync(x => x.Prioridad, x => x.Cantidad);
        }

        public async Task<List<Ticket>> ListarTodosLosTicketsAsync()
        {
            return await _contexto.Tickets
                .Include(t => t.Categoria)
                .Include(t => t.Prioridad)
                .Include(t => t.Estado)
                .Include(t => t.Solicitante)
                .OrderByDescending(t => t.FechaCreacion)
                .ToListAsync();
        }

        public async Task<List<Ticket>> ListarTicketsPorUsuarioAsync(int idUsuario)
        {
            return await _contexto.Tickets
                .Include(t => t.Categoria)
                .Include(t => t.Prioridad)
                .Include(t => t.Estado)
                .Where(t => t.IdSolicitante == idUsuario)
                .OrderByDescending(t => t.FechaCreacion)
                .ToListAsync();
        }

        public async Task<Ticket?> ObtenerTicketPorIdAsync(int id)
        {
            return await _contexto.Tickets
                .Include(t => t.Categoria)
                .Include(t => t.Prioridad)
                .Include(t => t.Estado)
                .Include(t => t.Solicitante)
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

        public async Task<List<Categoria>> ListarCategoriasAsync()
        {
            return await _contexto.Categorias
                .AsNoTracking()
                .Where(c => c.Activo == 1)
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }

        public async Task<Ticket> GuardarTicketAsync(Ticket ticket)
        {
            if (ticket.IdImpacto == 1 && ticket.IdUrgencia == 1) ticket.IdPrioridad = 1;
            else if (ticket.IdImpacto == 1 || ticket.IdUrgencia == 1) ticket.IdPrioridad = 2;
            else ticket.IdPrioridad = 3;

            if (ticket.IdTicket == 0)
            {
                ticket.FechaCreacion = DateTime.Now;
                ticket.IdEstado = 1;
                int horasSLA = ticket.IdPrioridad switch { 1 => 4, 2 => 24, _ => 72 };
                ticket.FechaLimite = DateTime.Now.AddHours(horasSLA);
                _contexto.Tickets.Add(ticket);
            }
            else { _contexto.Tickets.Update(ticket); }
            await _contexto.SaveChangesAsync();
            return ticket;
        }
    }
}