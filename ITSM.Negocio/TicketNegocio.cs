using Microsoft.EntityFrameworkCore;
using ITSM.Datos;
using ITSM.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            // Incluimos ("Join") las tablas relacionadas para mostrar nombres en lugar de IDs
            return await _contexto.Tickets
                .Include(t => t.Solicitante)
                .Include(t => t.Categoria)
                .Include(t => t.Prioridad)
                .Include(t => t.Estado)
                .OrderByDescending(t => t.FechaCreacion) // Lo más reciente primero
                .ToListAsync();
        }

        public async Task<Ticket?> ObtenerTicketPorIdAsync(int id)
        {
            return await _contexto.Tickets
                .Include(t => t.Solicitante)
                .Include(t => t.Categoria)
                .Include(t => t.Prioridad)
                .Include(t => t.Estado)
                .Include(t => t.ActivoRelacionado) // También traemos el activo si existe
                .FirstOrDefaultAsync(t => t.IdTicket == id);
        }

        // --- MÉTODOS PARA LLENAR COMBOS (DROPDOWNS) ---

        public async Task<List<Categoria>> ListarCategoriasAsync()
        {
            return await _contexto.Categorias
                .Where(c => c.Activo == 1) // Solo activas (int 1 = true en tu BD)
                .ToListAsync();
        }

        public async Task<List<Prioridad>> ListarPrioridadesAsync()
        {
            return await _contexto.Prioridades.ToListAsync();
        }

        public async Task<List<EstadoTicket>> ListarEstadosAsync()
        {
            return await _contexto.EstadosTicket.ToListAsync();
        }

        // --- MÉTODOS DE ACCIÓN (ESCRITURA) ---

        public async Task<Ticket> GuardarTicketAsync(Ticket ticket)
        {
            // LÓGICA DE TRAZABILIDAD (TESIS): 
            // Si viene un Código Patrimonial, buscamos el ID del Activo correspondiente
            if (!string.IsNullOrEmpty(ticket.CodigoPatrimonial))
            {
                var activoEncontrado = await _contexto.Activos
                    .FirstOrDefaultAsync(a => a.CodPatrimonial == ticket.CodigoPatrimonial);

                if (activoEncontrado != null)
                {
                    ticket.IdActivo = activoEncontrado.IdActivo;
                }
                // Si no se encuentra, el IdActivo queda null, pero el ticket se guarda igual
            }

            if (ticket.IdTicket == 0)
            {
                // Nuevo Ticket
                ticket.FechaCreacion = DateTime.Now;

                // Regla de Negocio ITIL: Todo ticket nace "Abierto" si no se especifica
                if (ticket.IdEstado == 0) ticket.IdEstado = 1; // 1 = Abierto

                // Regla de Negocio: Prioridad por defecto Media si no se especifica
                if (ticket.IdPrioridad == 0) ticket.IdPrioridad = 2; // 2 = Media

                _contexto.Tickets.Add(ticket);
            }
            else
            {
                // Actualizar Ticket existente
                _contexto.Tickets.Update(ticket);
            }

            await _contexto.SaveChangesAsync();
            return ticket;
        }

        // Método útil para cerrar tickets rápidamente
        public async Task CerrarTicketAsync(int idTicket)
        {
            var ticket = await _contexto.Tickets.FindAsync(idTicket);
            if (ticket != null)
            {
                ticket.IdEstado = 4; // 4 = Cerrado (Según tu script SQL)
                ticket.FechaCierre = DateTime.Now;
                await _contexto.SaveChangesAsync();
            }
        }
    }
}