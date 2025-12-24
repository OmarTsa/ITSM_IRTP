using Microsoft.EntityFrameworkCore;
using ITSM.Datos;
using ITSM.Entidades;
using ITSM.Entidades.DTOs; // Asegúrate de tener el namespace de DTOs

namespace ITSM.Negocio
{
    public class TicketNegocio
    {
        private readonly ContextoBD _context;

        public TicketNegocio(ContextoBD context)
        {
            _context = context;
        }

        // --- GESTIÓN DE TICKETS ---

        public async Task<List<Ticket>> ListarTicketsAsync()
        {
            return await _context.Tickets
                .Include(t => t.Solicitante)
                .Include(t => t.Especialista)
                .Include(t => t.Prioridad)
                .Include(t => t.Categoria)
                .Include(t => t.Estado)
                .Include(t => t.ActivoAfectado) // Incluimos el activo para que se vea en la grilla
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
                .Where(t => t.IdSolicitante == idUsuario)
                .OrderByDescending(t => t.FechaCreacion)
                .ToListAsync();
        }

        public async Task<Ticket?> ObtenerTicketPorIdAsync(int idTicket)
        {
            return await _context.Tickets
                .Include(t => t.Solicitante)
                .Include(t => t.Especialista)
                .Include(t => t.ActivoAfectado)
                    .ThenInclude(a => a.TipoActivo) // Para mostrar "Laptop - Serie 123"
                .Include(t => t.Categoria)
                .Include(t => t.Prioridad)
                .Include(t => t.Estado)
                .FirstOrDefaultAsync(t => t.IdTicket == idTicket);
        }

        public async Task RegistrarTicketAsync(Ticket ticket)
        {
            var usuario = await _context.Usuarios.FindAsync(ticket.IdSolicitante);
            if (usuario == null || usuario.Estado != 1)
                throw new Exception("El solicitante no existe o está inactivo.");

            // Lógica de Negocio: Calcular Prioridad basada en Matriz ITIL
            // Si la base de datos no tiene el Trigger o para asegurar en código:
            // ticket.IdPrioridad = CalcularPrioridad(ticket.IdImpacto, ticket.IdUrgencia);

            ticket.FechaCreacion = DateTime.Now;
            ticket.IdEstado = 1; // Abierto

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarTicketAsync(Ticket ticket)
        {
            var ticketDb = await _context.Tickets.FindAsync(ticket.IdTicket);
            if (ticketDb == null) throw new Exception("Ticket no encontrado");

            ticketDb.IdEstado = ticket.IdEstado;
            ticketDb.IdEspecialista = ticket.IdEspecialista;
            ticketDb.FechaAsignacion = ticket.FechaAsignacion;
            ticketDb.FechaCierre = ticket.FechaCierre;
            ticketDb.IdPrioridad = ticket.IdPrioridad;
            ticketDb.NotasCierre = ticket.NotasCierre;

            await _context.SaveChangesAsync();
        }

        public async Task AsignarTicketAsync(int idTicket, int idEspecialista)
        {
            var ticket = await _context.Tickets.FindAsync(idTicket);
            if (ticket == null) throw new Exception("Ticket no encontrado");

            ticket.IdEspecialista = idEspecialista;
            ticket.IdEstado = 2; // En Proceso
            ticket.FechaAsignacion = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        // --- GESTIÓN DE DETALLES (COMENTARIOS) ---

        public async Task AgregarComentarioAsync(TicketDetalle detalle)
        {
            detalle.FechaRegistro = DateTime.Now;
            _context.TicketDetalles.Add(detalle);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TicketDetalle>> ListarDetallesTicketAsync(int idTicket)
        {
            return await _context.TicketDetalles
                .Include(d => d.Usuario) // Para saber quién comentó
                .Where(d => d.IdTicket == idTicket)
                .OrderBy(d => d.FechaRegistro)
                .ToListAsync();
        }

        // --- CATÁLOGOS AUXILIARES ---

        public async Task<List<Categoria>> ListarCategoriasAsync()
        {
            return await _context.Categorias.Where(c => c.Activo == 1).ToListAsync();
        }

        public async Task<List<Prioridad>> ListarPrioridadesAsync()
        {
            return await _context.Prioridades.ToListAsync();
        }

        public async Task<List<EstadoTicket>> ListarEstadosAsync()
        {
            return await _context.Estados.ToListAsync();
        }

        // --- DASHBOARD & KPIS (Requerido por TicketController) ---

        public async Task<DashboardKpi> ObtenerKpisAsync()
        {
            var kpi = new DashboardKpi();

            kpi.TotalTickets = await _context.Tickets.CountAsync();
            kpi.Pendientes = await _context.Tickets.CountAsync(t => t.IdEstado == 1 || t.IdEstado == 2);
            kpi.Resueltos = await _context.Tickets.CountAsync(t => t.IdEstado == 4 || t.IdEstado == 5);

            // Ejemplo de cálculo simple
            if (kpi.TotalTickets > 0)
                kpi.PorcentajeAtencion = (int)((double)kpi.Resueltos / kpi.TotalTickets * 100);
            else
                kpi.PorcentajeAtencion = 0;

            return kpi;
        }
    }
}