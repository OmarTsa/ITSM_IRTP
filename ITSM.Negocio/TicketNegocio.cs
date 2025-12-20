using Microsoft.EntityFrameworkCore;
using ITSM.Datos;
using ITSM.Entidades;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace ITSM.Negocio
{
    public class TicketNegocio
    {
        private readonly ContextoBD _contexto;

        public TicketNegocio(ContextoBD contexto)
        {
            _contexto = contexto;
        }

        public async Task<List<Ticket>> ListarTodosLosTicketsAsync()
        {
            return await _contexto.Tickets
                .Include(t => t.Categoria).Include(t => t.Prioridad)
                .Include(t => t.Estado).Include(t => t.Solicitante)
                .OrderByDescending(t => t.FechaCreacion).ToListAsync();
        }

        public async Task<Ticket?> ObtenerTicketPorIdAsync(int id)
        {
            return await _contexto.Tickets
                .Include(t => t.Categoria).Include(t => t.Prioridad)
                .Include(t => t.Estado).Include(t => t.Solicitante)
                .Include(t => t.ActivoRelacionado)
                .FirstOrDefaultAsync(t => t.IdTicket == id);
        }

        public async Task CambiarEstadoTicketAsync(int idTicket, int nuevoEstado, int idUsuarioOperador, string? notas = null)
        {
            var ticket = await _contexto.Tickets.FindAsync(idTicket);
            if (ticket != null)
            {
                ticket.IdEstado = nuevoEstado;
                // CORRECCIÓN: Comparación numérica estricta (int == int)
                if (nuevoEstado == 4 || nuevoEstado == 5)
                {
                    ticket.FechaCierre = DateTime.Now;
                    ticket.NotasCierre = notas;
                }
                await _contexto.SaveChangesAsync();
            }
        }

        public async Task<Dictionary<string, int>> ObtenerTicketsPorEstadoAsync()
        {
            var datos = await _contexto.Tickets.Include(t => t.Estado).ToListAsync();
            return datos.GroupBy(t => t.Estado?.Nombre ?? "Sin Estado")
                        .ToDictionary(g => g.Key, g => g.Count());
        }

        public async Task<List<Categoria>> ListarCategoriasAsync()
        {
            return await _contexto.Categorias.ToListAsync();
        }

        public async Task<bool> GuardarTicketAsync(Ticket ticket)
        {
            try
            {
                ticket.FechaCreacion = DateTime.Now;
                if (ticket.IdEstado == 0) ticket.IdEstado = 1;
                _contexto.Tickets.Add(ticket);
                await _contexto.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Ticket>> ListarTicketsPorUsuarioAsync(int idUsuario)
        {
            return await _contexto.Tickets
                .Include(t => t.Estado)
                .Where(t => t.IdSolicitante == idUsuario)
                .ToListAsync();
        }

        public async Task<List<Usuario>> ListarUsuariosAsync()
        {
            // CORRECCIÓN: Comparación de int (Estado) contra int (1)
            return await _contexto.Usuarios
                .Where(u => u.Estado == 1)
                .ToListAsync();
        }
    }
}