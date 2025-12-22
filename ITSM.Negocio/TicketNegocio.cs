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
                .Include(t => t.Especialista) // Incluimos al técnico asignado
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

        public async Task<List<Prioridad>> ListarPrioridades()
        {
            return await _contexto.Prioridades.ToListAsync();
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

        public async Task<DashboardKpi> ObtenerKpisAsync(int idUsuario)
        {
            var kpi = new DashboardKpi();
            var todos = await _contexto.Tickets.AsNoTracking().ToListAsync();

            kpi.TotalTickets = todos.Count;
            kpi.TicketsAbiertos = todos.Count(t => t.IdEstado == 1);
            kpi.TicketsResueltos = todos.Count(t => t.IdEstado == 4);
            kpi.TicketsCriticos = todos.Count(t => t.IdPrioridad == 1 && t.IdEstado != 4 && t.IdEstado != 5);
            kpi.MisAsignados = todos.Count(t => t.IdEspecialista == idUsuario && t.IdEstado != 5);

            return kpi;
        }

        // --- NUEVOS MÉTODOS PARA DETALLE DE TICKET ---

        public async Task<List<TicketDetalle>> ListarDetallesTicket(int idTicket)
        {
            return await _contexto.TicketDetalles
                .Include(d => d.Usuario) // Importante: Traer nombre de quien comentó
                .Where(d => d.IdTicket == idTicket)
                .OrderBy(d => d.FechaRegistro)
                .ToListAsync();
        }

        public async Task AgregarComentario(TicketDetalle detalle)
        {
            detalle.FechaRegistro = DateTime.Now;
            _contexto.TicketDetalles.Add(detalle);
            await _contexto.SaveChangesAsync();
        }
    }
}