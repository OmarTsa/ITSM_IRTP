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
                .OrderByDescending(t => t.IdPrioridad) // Primero lo crítico
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

        // --- ESCRITURA (CORE ITIL) ---
        public async Task<Ticket> GuardarTicketAsync(Ticket ticket)
        {
            // 1. Buscamos Activo en CMDB
            if (!string.IsNullOrEmpty(ticket.CodigoPatrimonial))
            {
                var activo = await _contexto.Activos
                    .FirstOrDefaultAsync(a => a.CodPatrimonial == ticket.CodigoPatrimonial);
                if (activo != null) ticket.IdActivo = activo.IdActivo;
            }

            // 2. Calculamos Prioridad (Matriz Impacto x Urgencia)
            if (ticket.IdImpacto == 1 && ticket.IdUrgencia == 1) ticket.IdPrioridad = 1; // Crítico
            else if (ticket.IdImpacto == 1 || ticket.IdUrgencia == 1) ticket.IdPrioridad = 2; // Alto
            else if (ticket.IdImpacto == 2 && ticket.IdUrgencia == 2) ticket.IdPrioridad = 3; // Medio
            else ticket.IdPrioridad = 3; // Mínimo (Si no tienes prioridad 4 en BD)

            if (ticket.IdTicket == 0)
            {
                ticket.FechaCreacion = DateTime.Now;
                ticket.IdEstado = 1; // Abierto

                // 3. Calculamos SLA (Horas Límite)
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

        // Listas Auxiliares
        public async Task<List<Categoria>> ListarCategoriasAsync() => await _contexto.Categorias.ToListAsync();
        public async Task<List<Prioridad>> ListarPrioridadesAsync() => await _contexto.Prioridades.ToListAsync();
        public async Task<List<EstadoTicket>> ListarEstadosAsync() => await _contexto.EstadosTicket.ToListAsync();
    }
}