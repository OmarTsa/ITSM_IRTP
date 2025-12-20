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

        // --- CONSULTA DE CATEGORÍAS (EL PUNTO DE FALLA) ---
        public async Task<List<Categoria>> ListarCategoriasAsync()
        {
            // Forzamos la lectura fresca de la tabla HD_CATEGORIAS
            return await _contexto.Categorias.AsNoTracking().ToListAsync();
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
                .FirstOrDefaultAsync(t => t.IdTicket == id);
        }

        public async Task<Ticket> GuardarTicketAsync(Ticket ticket)
        {
            // Matriz de Prioridad ITIL
            if (ticket.IdImpacto == 1 && ticket.IdUrgencia == 1) ticket.IdPrioridad = 1;
            else if (ticket.IdImpacto == 1 || ticket.IdUrgencia == 1) ticket.IdPrioridad = 2;
            else ticket.IdPrioridad = 3;

            if (ticket.IdTicket == 0)
            {
                ticket.FechaCreacion = DateTime.Now;
                ticket.IdEstado = 1;
                ticket.FechaLimite = DateTime.Now.AddHours(ticket.IdPrioridad == 1 ? 4 : 24);
                _contexto.Tickets.Add(ticket);
            }
            else
            {
                _contexto.Tickets.Update(ticket);
            }

            await _contexto.SaveChangesAsync();
            return ticket;
        }

        public async Task CambiarEstadoTicketAsync(int idTicket, int nuevoEstado, int idUsuarioOperador, string? notas = null)
        {
            var ticket = await _contexto.Tickets.FindAsync(idTicket);
            if (ticket != null)
            {
                ticket.IdEstado = nuevoEstado;
                if (nuevoEstado == 4)
                {
                    ticket.FechaCierre = DateTime.Now;
                    ticket.NotasCierre = notas;
                    ticket.CodigoCierre = "Resuelto";
                }
                await _contexto.SaveChangesAsync();
            }
        }

        public async Task<List<Prioridad>> ListarPrioridadesAsync() => await _contexto.Prioridades.ToListAsync();
        public async Task<List<EstadoTicket>> ListarEstadosAsync() => await _contexto.EstadosTicket.ToListAsync();
    }
}