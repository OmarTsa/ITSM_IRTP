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

        // Listar TODOS (Admin)
        public async Task<List<Ticket>> ListarTicketsAsync()
        {
            return await _contexto.Tickets
                .Include(t => t.Solicitante)
                .Include(t => t.Categoria)
                .Include(t => t.Prioridad)
                .Include(t => t.Estado)
                .OrderByDescending(t => t.IdPrioridad) // ITIL: Primero Urgente
                .ThenByDescending(t => t.FechaCreacion)
                .ToListAsync();
        }

        // Listar POR USUARIO
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

        public async Task<Ticket> GuardarTicketAsync(Ticket ticket)
        {
            // 1. Vincular Activo (CMDB)
            if (!string.IsNullOrEmpty(ticket.CodigoPatrimonial))
            {
                var activo = await _contexto.Activos
                    .FirstOrDefaultAsync(a => a.CodPatrimonial == ticket.CodigoPatrimonial);
                if (activo != null) ticket.IdActivo = activo.IdActivo;
            }

            // 2. MATRIZ DE PRIORIDAD ITIL
            // 1(Alto)+1(Alto) = 1(Crítico). Resto disminuye.
            if (ticket.IdImpacto == 1 && ticket.IdUrgencia == 1) ticket.IdPrioridad = 1;
            else if (ticket.IdImpacto == 1 || ticket.IdUrgencia == 1) ticket.IdPrioridad = 2;
            else if (ticket.IdImpacto == 2 && ticket.IdUrgencia == 2) ticket.IdPrioridad = 3;
            else ticket.IdPrioridad = 3; // Mínimo 3 (Baja)

            if (ticket.IdTicket == 0)
            {
                ticket.FechaCreacion = DateTime.Now;
                ticket.IdEstado = 1; // Abierto

                // 3. CÁLCULO SLA (Horas límite)
                int horas = ticket.IdPrioridad switch
                {
                    1 => 4,   // Critico
                    2 => 8,   // Alto
                    3 => 24,  // Medio
                    _ => 48   // Bajo
                };
                ticket.FechaLimite = DateTime.Now.AddHours(horas);

                _contexto.Tickets.Add(ticket);
            }
            else
            {
                _contexto.Tickets.Update(ticket);
            }

            await _contexto.SaveChangesAsync();
            return ticket;
        }

        // Combos
        public async Task<List<Categoria>> ListarCategoriasAsync() => await _contexto.Categorias.Where(c => c.Activo == 1).ToListAsync();
        public async Task<List<Prioridad>> ListarPrioridadesAsync() => await _contexto.Prioridades.ToListAsync();
        public async Task<List<EstadoTicket>> ListarEstadosAsync() => await _contexto.EstadosTicket.ToListAsync();
    }
}