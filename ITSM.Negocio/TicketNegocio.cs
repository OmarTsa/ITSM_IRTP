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

        // --- LECTURA ---
        public async Task<List<Ticket>> ListarTicketsAsync()
        {
            return await _contexto.Tickets
                .Include(t => t.Solicitante)
                .Include(t => t.Categoria)
                .Include(t => t.Prioridad)
                .Include(t => t.Estado)
                .OrderByDescending(t => t.IdPrioridad)
                .ThenByDescending(t => t.FechaCreacion)
                .ToListAsync();
        }

        public async Task<List<Ticket>> ListarTicketsPorUsuarioAsync(int idUsuario)
        {
            return await _contexto.Tickets
                .Include(t => t.Categoria)
                .Include(t => t.Prioridad)
                .Include(t => t.Estado)
                .Where(t => t.IdSolicitante == idUsuario)
                .OrderByDescending(t => t.IdPrioridad)
                .ThenByDescending(t => t.FechaCreacion)
                .ToListAsync();
        }

        public async Task<Ticket?> ObtenerTicketPorIdAsync(int id)
        {
            return await _contexto.Tickets
                .Include(t => t.Solicitante)
                .Include(t => t.Categoria)
                .Include(t => t.Prioridad)
                .Include(t => t.Estado)
                .Include(t => t.ActivoRelacionado)
                .FirstOrDefaultAsync(t => t.IdTicket == id);
        }

        // --- ESCRITURA ---
        public async Task<Ticket> GuardarTicketAsync(Ticket ticket)
        {
            if (!string.IsNullOrEmpty(ticket.CodigoPatrimonial))
            {
                var activo = await _contexto.Activos
                    .FirstOrDefaultAsync(a => a.CodPatrimonial == ticket.CodigoPatrimonial);
                if (activo != null) ticket.IdActivo = activo.IdActivo;
            }

            // Matriz Prioridad
            if (ticket.IdImpacto == 1 && ticket.IdUrgencia == 1) ticket.IdPrioridad = 1;
            else if (ticket.IdImpacto == 1 || ticket.IdUrgencia == 1) ticket.IdPrioridad = 2;
            else if (ticket.IdImpacto == 2 && ticket.IdUrgencia == 2) ticket.IdPrioridad = 3;
            else ticket.IdPrioridad = 3;

            if (ticket.IdTicket == 0)
            {
                ticket.FechaCreacion = DateTime.Now;
                ticket.IdEstado = 1; // Abierto

                int horasSLA = ticket.IdPrioridad switch
                {
                    1 => 4,
                    2 => 8,
                    3 => 24,
                    _ => 48
                };
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

        // --- NUEVO MÉTODO: CAMBIO DE ESTADO ---
        public async Task CambiarEstadoTicketAsync(int idTicket, int nuevoEstado, int idUsuarioOperador, string? notas = null)
        {
            var ticket = await _contexto.Tickets.FindAsync(idTicket);
            if (ticket == null) throw new Exception("Ticket no encontrado");

            ticket.IdEstado = nuevoEstado;

            // Si se cierra el ticket
            if (nuevoEstado == 4)
            {
                ticket.FechaCierre = DateTime.Now;
                ticket.CodigoCierre = "Resuelto";
                ticket.NotasCierre = notas;
            }

            await _contexto.SaveChangesAsync();
        }

        // --- COMBOS ---
        public async Task<List<Categoria>> ListarCategoriasAsync() => await _contexto.Categorias.ToListAsync();
        public async Task<List<Prioridad>> ListarPrioridadesAsync() => await _contexto.Prioridades.ToListAsync();
        public async Task<List<EstadoTicket>> ListarEstadosAsync() => await _contexto.EstadosTicket.ToListAsync();
    }
}