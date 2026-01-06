using Microsoft.EntityFrameworkCore;
using ITSM.Datos;
using ITSM.Entidades;
using ITSM.Entidades.DTOs;

namespace ITSM.Negocio
{
    public class TicketNegocio
    {
        private readonly ContextoBD _context;

        public TicketNegocio(ContextoBD context)
        {
            _context = context;
        }

        // ----------------------------
        // GESTIÓN DE TICKETS
        // ----------------------------

        public async Task<List<Ticket>> ListarTicketsAsync()
        {
            return await _context.Tickets
                .Include(t => t.Solicitante)
                    .ThenInclude(s => s.Area)
                .Include(t => t.Especialista)
                .Include(t => t.Prioridad)
                .Include(t => t.Categoria)
                .Include(t => t.Estado)
                .Include(t => t.ActivoAfectado)
                    .ThenInclude(a => a.TipoActivo)
                .Include(t => t.AreaSolicitante)
                .OrderByDescending(t => t.FechaCreacion)
                .ToListAsync();
        }

        public async Task<List<Ticket>> ListarMisTicketsAsync(int idUsuario)
        {
            return await _context.Tickets
                .Include(t => t.Prioridad)
                .Include(t => t.Categoria)
                .Include(t => t.Estado)
                .Include(t => t.ActivoAfectado)
                .Include(t => t.AreaSolicitante)
                .Where(t => t.IdSolicitante == idUsuario)
                .OrderByDescending(t => t.FechaCreacion)
                .ToListAsync();
        }

        public async Task<Ticket?> ObtenerTicketPorIdAsync(int idTicket)
        {
            return await _context.Tickets
                .Include(t => t.Solicitante)
                    .ThenInclude(s => s.Area)
                .Include(t => t.Especialista)
                .Include(t => t.ActivoAfectado)
                    .ThenInclude(a => a.TipoActivo)
                .Include(t => t.Categoria)
                .Include(t => t.Prioridad)
                .Include(t => t.Estado)
                .Include(t => t.AreaSolicitante)
                .FirstOrDefaultAsync(t => t.IdTicket == idTicket);
        }

        public async Task<Ticket> RegistrarTicketAsync(Ticket ticket)
        {
            Usuario? usuario = await _context.Usuarios
                .Include(u => u.Area)
                .FirstOrDefaultAsync(u => u.IdUsuario == ticket.IdSolicitante);

            if (usuario == null || usuario.Estado != 1)
                throw new Exception("El solicitante no existe o está inactivo.");

            if (ticket.IdAreaSolicitante == null && usuario.IdArea > 0)
            {
                ticket.IdAreaSolicitante = usuario.IdArea;
            }

            if (ticket.IdActivoAfectado.HasValue)
            {
                var activo = await _context.Activos
                    .FirstOrDefaultAsync(a => a.IdActivo == ticket.IdActivoAfectado);

                if (activo == null)
                    throw new Exception("El activo especificado no existe.");
            }

            ticket.FechaCreacion = DateTime.Now;
            ticket.IdEstado = 1;

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            var ticketCreado = await ObtenerTicketPorIdAsync(ticket.IdTicket);
            return ticketCreado ?? ticket;
        }

        public async Task ActualizarTicketAsync(Ticket ticket)
        {
            var ticketDb = await _context.Tickets.FindAsync(ticket.IdTicket);
            if (ticketDb == null) throw new Exception("Ticket no encontrado");

            int estadoAnterior = ticketDb.IdEstado;
            int estadoNuevo = ticket.IdEstado;

            ticketDb.IdEstado = ticket.IdEstado;
            ticketDb.IdEspecialista = ticket.IdEspecialista;
            ticketDb.FechaAsignacion = ticket.FechaAsignacion;
            ticketDb.FechaCierre = ticket.FechaCierre;
            ticketDb.IdPrioridad = ticket.IdPrioridad;
            ticketDb.NotasCierre = ticket.NotasCierre;
            ticketDb.IdActivoAfectado = ticket.IdActivoAfectado;
            ticketDb.IdAreaSolicitante = ticket.IdAreaSolicitante;

            const int ESTADO_CERRADO = 4;

            if (estadoNuevo == ESTADO_CERRADO)
            {
                if (ticketDb.IdEspecialista == null)
                    throw new Exception("No se puede cerrar el ticket sin especialista asignado.");

                if (ticketDb.IdSolicitante == 0)
                    throw new Exception("No se puede cerrar el ticket sin solicitante.");

                ticketDb.FechaCierre ??= DateTime.Now;
            }

            await _context.SaveChangesAsync();
        }

        public async Task AsignarTicketAsync(int idTicket, int idEspecialista)
        {
            var ticket = await _context.Tickets.FindAsync(idTicket);
            if (ticket == null) throw new Exception("Ticket no encontrado");

            ticket.IdEspecialista = idEspecialista;
            ticket.IdEstado = 2;
            ticket.FechaAsignacion = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        // ----------------------------
        // COMENTARIOS
        // ----------------------------

        public async Task AgregarComentarioAsync(TicketComentario comentario)
        {
            comentario.FechaComentario = DateTime.Now;
            _context.TicketComentarios.Add(comentario);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TicketComentario>> ListarDetallesTicketAsync(int idTicket)
        {
            return await _context.TicketComentarios
                .Include(c => c.Usuario)
                .Where(c => c.IdTicket == idTicket && c.Eliminado == 0)
                .OrderBy(c => c.FechaComentario)
                .ToListAsync();
        }

        // ----------------------------
        // CATÁLOGOS AUXILIARES
        // ----------------------------

        public async Task<List<Categoria>> ListarCategoriasAsync()
        {
            return await _context.Categorias
                .Where(c => c.Activo == 1)
                .ToListAsync();
        }

        public async Task<List<Prioridad>> ListarPrioridadesAsync()
        {
            return await _context.Prioridades.ToListAsync();
        }

        public async Task<List<EstadoTicket>> ListarEstadosAsync()
        {
            return await _context.Estados.ToListAsync();
        }

        // ----------------------------
        // DASHBOARD & KPIS
        // ----------------------------

        public async Task<DashboardKpi> ObtenerKpisAsync()
        {
            var kpi = new DashboardKpi();

            kpi.TotalTickets = await _context.Tickets.CountAsync();
            kpi.Pendientes = await _context.Tickets.CountAsync(t => t.IdEstado == 1 || t.IdEstado == 2);
            kpi.Resueltos = await _context.Tickets.CountAsync(t => t.IdEstado == 4 || t.IdEstado == 5);

            if (kpi.TotalTickets > 0)
                kpi.PorcentajeAtencion = (int)((double)kpi.Resueltos / kpi.TotalTickets * 100);
            else
                kpi.PorcentajeAtencion = 0;

            return kpi;
        }
    }
}
