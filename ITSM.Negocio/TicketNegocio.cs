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

        // Método para listar categorías activas
        public async Task<List<Categoria>> ListarCategoriasAsync()
        {
            return await _contexto.Categorias
                .AsNoTracking()
                .Where(c => c.Activo == 1)
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }

        // Método para listar tickets de un usuario específico
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

        // Método para obtener un ticket por su ID
        public async Task<Ticket?> ObtenerTicketPorIdAsync(int id)
        {
            return await _contexto.Tickets
                .Include(t => t.Categoria)
                .Include(t => t.Prioridad)
                .Include(t => t.Estado)
                .FirstOrDefaultAsync(t => t.IdTicket == id);
        }

        // Método para guardar o actualizar tickets con lógica ITIL
        public async Task<Ticket> GuardarTicketAsync(Ticket ticket)
        {
            // Lógica de Prioridad (Impacto vs Urgencia)
            if (ticket.IdImpacto == 1 && ticket.IdUrgencia == 1) ticket.IdPrioridad = 1; // Alta
            else if (ticket.IdImpacto == 1 || ticket.IdUrgencia == 1) ticket.IdPrioridad = 2; // Media
            else ticket.IdPrioridad = 3; // Baja

            if (ticket.IdTicket == 0)
            {
                ticket.FechaCreacion = DateTime.Now;
                ticket.IdEstado = 1; // Abierto

                // Cálculo de SLA (4h para prioridad 1, 24h para 2, 72h para 3)
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

        // Método para cambiar el estado del ticket
        public async Task CambiarEstadoTicketAsync(int idTicket, int nuevoEstado, int idUsuarioOperador, string? notas = null)
        {
            var ticket = await _contexto.Tickets.FindAsync(idTicket);
            if (ticket != null)
            {
                ticket.IdEstado = nuevoEstado;
                if (nuevoEstado == 4) // 4 = Resuelto
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