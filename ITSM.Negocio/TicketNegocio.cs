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

        public async Task<List<Ticket>> ObtenerTickets()
        {
            return await _contexto.Tickets
                .Include(t => t.Categoria)
                .Include(t => t.Estado)
                .Include(t => t.Prioridad)
                .OrderByDescending(t => t.FechaCreacion)
                .ToListAsync();
        }

        public async Task<Ticket?> ObtenerTicketPorId(int id)
        {
            return await _contexto.Tickets
                .Include(t => t.Categoria)
                .Include(t => t.Estado)
                .Include(t => t.Prioridad)
                .Include(t => t.Solicitante)
                .FirstOrDefaultAsync(t => t.IdTicket == id);
        }

        public async Task<List<Ticket>> ObtenerTicketsPorUsuario(int idUsuario)
        {
            return await _contexto.Tickets
                .Include(t => t.Categoria)
                .Include(t => t.Estado)
                .Where(t => t.IdSolicitante == idUsuario)
                .OrderByDescending(t => t.FechaCreacion)
                .ToListAsync();
        }

        public async Task<List<Categoria>> ListarCategorias()
        {
            return await _contexto.Categorias.Where(c => c.Activo == 1).ToListAsync();
        }

        public async Task GuardarTicket(Ticket ticket)
        {
            if (ticket.IdTicket == 0)
            {
                ticket.FechaCreacion = DateTime.Now;
                _contexto.Tickets.Add(ticket);
            }
            else
            {
                _contexto.Tickets.Update(ticket);
            }
            await _contexto.SaveChangesAsync();
        }

        // --- MÉTODOS DE SOPORTE Y KPI ---

        public async Task<List<Ticket>> ListarTodosLosTicketsAsync()
        {
            return await _contexto.Tickets
                .Include(t => t.Categoria)
                .Include(t => t.Estado)
                .Include(t => t.Prioridad)
                .Include(t => t.Solicitante)
                .OrderByDescending(t => t.FechaCreacion)
                .ToListAsync();
        }

        public async Task<List<Activo>> ListarActivosAsync()
        {
            return await _contexto.Activos
                .Include(a => a.UsuarioAsignado)
                .ToListAsync();
        }

        // NUEVO: CÁLCULO DE DASHBOARD
        public async Task<DashboardKpi> ObtenerKpisAsync(int idUsuario)
        {
            var kpi = new DashboardKpi();

            // Traemos todos los tickets (si son muchos, optimizar con CountAsync separados)
            var todos = await _contexto.Tickets.AsNoTracking().ToListAsync();

            kpi.TotalTickets = todos.Count;
            kpi.TicketsAbiertos = todos.Count(t => t.IdEstado == 1); // 1 = Abierto
            kpi.TicketsResueltos = todos.Count(t => t.IdEstado == 4); // 4 = Resuelto

            // Críticos: Prioridad 1 (Alta) y que no estén cerrados
            kpi.TicketsCriticos = todos.Count(t => t.IdPrioridad == 1 && t.IdEstado != 4 && t.IdEstado != 5);

            // Mis asignados (si el usuario es técnico)
            kpi.MisAsignados = todos.Count(t => t.IdEspecialista == idUsuario && t.IdEstado != 5);

            return kpi;
        }
    }
}