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

        // Obtiene las categorías activas directamente de la tabla HD_CATEGORIAS
        public async Task<List<Categoria>> ListarCategoriasAsync()
        {
            return await _contexto.Categorias
                .AsNoTracking()
                .Where(c => c.Activo == 1)
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }

        // Guarda o actualiza un ticket con lógica de prioridad ITIL
        public async Task<Ticket> GuardarTicketAsync(Ticket ticket)
        {
            // Matriz de Prioridad (Impacto vs Urgencia)
            if (ticket.IdImpacto == 1 && ticket.IdUrgencia == 1) ticket.IdPrioridad = 1;
            else if (ticket.IdImpacto == 1 || ticket.IdUrgencia == 1) ticket.IdPrioridad = 2;
            else ticket.IdPrioridad = 3;

            if (ticket.IdTicket == 0)
            {
                ticket.FechaCreacion = DateTime.Now;
                ticket.IdEstado = 1; // Abierto

                // Cálculo de SLA basado en prioridad
                int horasSLA = ticket.IdPrioridad switch { 1 => 4, 2 => 24, _ => 72 };
                ticket.FechaLimite = DateTime.Now.AddHours(horasSLA);

                _contexto.Tickets.Add(ticket);
            }
            else
            {
                _contexto.Tickets.Update(ticket);
            }

            await _contexto.SaveChangesAsync();
            return ticket;
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

        public async Task CambiarEstadoTicketAsync(int idTicket, int nuevoEstado, int idUsuarioOperador, string? notas = null)
        {
            var ticket = await _contexto.Tickets.FindAsync(idTicket);
            if (ticket != null)
            {
                ticket.IdEstado = nuevoEstado;
                if (nuevoEstado == 4) // Resuelto
                {
                    ticket.FechaCierre = DateTime.Now;
                    ticket.NotasCierre = notas;
                    ticket.CodigoCierre = "Resuelto";
                }
                await _contexto.SaveChangesAsync();
            }
        }
    }
}