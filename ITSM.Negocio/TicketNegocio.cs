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

        // --- MÉTODOS DE CONSULTA (LECTURA) ---

        public async Task<List<Ticket>> ListarTicketsAsync()
        {
            return await _contexto.Tickets
                .Include(t => t.Solicitante)
                .Include(t => t.Categoria)
                .Include(t => t.Prioridad)
                .Include(t => t.Estado)
                .OrderByDescending(t => t.IdPrioridad) // ITIL: Primero lo más urgente
                .ThenByDescending(t => t.FechaCreacion)
                .ToListAsync();
        }

        // NUEVO MÉTODO: Filtrar tickets de un usuario específico
        public async Task<List<Ticket>> ListarTicketsPorUsuarioAsync(int idUsuario)
        {
            return await _contexto.Tickets
                .Include(t => t.Categoria)
                .Include(t => t.Prioridad)
                .Include(t => t.Estado)
                .Where(t => t.IdSolicitante == idUsuario) // <-- FILTRO
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

        // --- MÉTODOS PARA LISTAS DESPLEGABLES ---

        public async Task<List<Categoria>> ListarCategoriasAsync() => await _contexto.Categorias.Where(c => c.Activo == 1).ToListAsync();
        public async Task<List<Prioridad>> ListarPrioridadesAsync() => await _contexto.Prioridades.ToListAsync();
        public async Task<List<EstadoTicket>> ListarEstadosAsync() => await _contexto.EstadosTicket.ToListAsync();

        // --- MÉTODOS DE ACCIÓN (ESCRITURA) ---

        public async Task<Ticket> GuardarTicketAsync(Ticket ticket)
        {
            // 1. Vincular Activo por Código Patrimonial (CMDB Link)
            if (!string.IsNullOrEmpty(ticket.CodigoPatrimonial))
            {
                var activo = await _contexto.Activos
                    .FirstOrDefaultAsync(a => a.CodPatrimonial == ticket.CodigoPatrimonial);
                if (activo != null) ticket.IdActivo = activo.IdActivo;
            }

            // 2. MATRIZ DE PRIORIZACIÓN ITIL (Impacto + Urgencia = Prioridad)
            if (ticket.IdImpacto == 1 && ticket.IdUrgencia == 1) ticket.IdPrioridad = 1; // Crítica
            else if (ticket.IdImpacto == 1 || ticket.IdUrgencia == 1) ticket.IdPrioridad = 2; // Alta
            else if (ticket.IdImpacto == 2 && ticket.IdUrgencia == 2) ticket.IdPrioridad = 3; // Media
            else ticket.IdPrioridad = 4; // Baja (Usaremos 3 si no existe 4 en BD)

            if (ticket.IdPrioridad > 3) ticket.IdPrioridad = 3;

            if (ticket.IdTicket == 0)
            {
                // NUEVO TICKET
                ticket.FechaCreacion = DateTime.Now;
                ticket.IdEstado = 1; // Abierto

                // 3. CÁLCULO DE SLA (Fecha Límite)
                int horasSLA = ticket.IdPrioridad switch
                {
                    1 => 4,   // Crítico: 4 horas
                    2 => 8,   // Alto: 8 horas
                    3 => 24,  // Medio: 24 horas
                    _ => 48   // Bajo
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

        public async Task CerrarTicketAsync(int idTicket, string notas, string codigoCierre = "Resuelto")
        {
            var ticket = await _contexto.Tickets.FindAsync(idTicket);
            if (ticket != null)
            {
                ticket.IdEstado = 4; // Cerrado
                ticket.FechaCierre = DateTime.Now;
                ticket.NotasCierre = notas;
                ticket.CodigoCierre = codigoCierre;
                await _contexto.SaveChangesAsync();
            }
        }
    }
}