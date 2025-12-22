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

        // --- MÉTODOS EXISTENTES (Déjalos igual) ---
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

        // --- MÉTODOS FALTANTES QUE CAUSAN EL ERROR (AGREGAR ESTOS) ---

        // 1. Método que pide tu página de Tickets
        public async Task<List<Ticket>> ListarTodosLosTicketsAsync()
        {
            // Reutilizamos la lógica o hacemos la consulta completa incluyendo al Solicitante
            return await _contexto.Tickets
                .Include(t => t.Categoria)
                .Include(t => t.Estado)
                .Include(t => t.Prioridad)
                .Include(t => t.Solicitante) // IMPORTANTE: Para mostrar el nombre del usuario
                .OrderByDescending(t => t.FechaCreacion)
                .ToListAsync();
        }

        // 2. Método que pide tu página de Inventario
        public async Task<List<Activo>> ListarActivosAsync()
        {
            return await _contexto.Activos
                .Include(a => a.UsuarioAsignado) // Para mostrar quién lo tiene asignado
                .ToListAsync();
        }
    }
}