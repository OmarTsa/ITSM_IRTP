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

        public async Task<List<Categoria>> ListarCategoriasAsync()
        {
            // Forzamos la carga sin filtros para ver los 5 ítems
            return await _contexto.Categorias.ToListAsync();
        }

        public async Task<List<Ticket>> ListarTicketsPorUsuarioAsync(int idUsuario)
        {
            return await _contexto.Tickets
                .Include(t => t.Categoria)
                .Include(t => t.Estado)
                .Where(t => t.IdSolicitante == idUsuario)
                .OrderByDescending(t => t.FechaCreacion)
                .ToListAsync();
        }

        public async Task<Ticket?> ObtenerTicketPorIdAsync(int id)
        {
            return await _contexto.Tickets
                .Include(t => t.Categoria)
                .Include(t => t.Estado)
                .FirstOrDefaultAsync(t => t.IdTicket == id);
        }

        public async Task<Ticket> GuardarTicketAsync(Ticket ticket)
        {
            if (ticket.IdTicket == 0)
            {
                ticket.FechaCreacion = DateTime.Now;
                ticket.IdEstado = 1;
                _contexto.Tickets.Add(ticket);
            }
            else
            {
                _contexto.Tickets.Update(ticket);
            }
            await _contexto.SaveChangesAsync();
            return ticket;
        }

        public async Task CambiarEstadoTicketAsync(int idTicket, int nuevoEstado, int idUsuario, string? notas)
        {
            var ticket = await _contexto.Tickets.FindAsync(idTicket);
            if (ticket != null)
            {
                ticket.IdEstado = nuevoEstado;
                if (nuevoEstado == 4)
                {
                    ticket.FechaCierre = DateTime.Now;
                    ticket.NotasCierre = notas;
                }
                await _contexto.SaveChangesAsync();
            }
        }
    }
}