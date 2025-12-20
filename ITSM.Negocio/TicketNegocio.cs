using Microsoft.EntityFrameworkCore;
using ITSM.Datos;
using ITSM.Entidades;

namespace ITSM.Negocio;

public class TicketNegocio
{
    private readonly ContextoBD _contexto;
    public TicketNegocio(ContextoBD contexto) => _contexto = contexto;

    public async Task<List<Categoria>> ListarCategoriasAsync()
        => await _contexto.Categorias.AsNoTracking().ToListAsync();

    public async Task<List<Ticket>> ListarTicketsPorUsuarioAsync(int idUsuario)
        => await _contexto.Tickets
            .Include(t => t.Estado)
            .Where(t => t.IdSolicitante == idUsuario)
            .AsNoTracking().ToListAsync();

    public async Task<List<Ticket>> ListarTodosLosTicketsAsync()
        => await _contexto.Tickets
            .Include(t => t.Estado)
            .Include(t => t.Categoria)
            .Include(t => t.Solicitante)
            .OrderByDescending(t => t.FechaCreacion)
            .AsNoTracking().ToListAsync();

    public async Task<List<Activo>> ListarActivosAsync()
        => await _contexto.Activos.Include(a => a.UsuarioAsignado).AsNoTracking().ToListAsync();

    public async Task<bool> GuardarTicketAsync(Ticket ticket)
    {
        _contexto.Tickets.Add(ticket);
        return await _contexto.SaveChangesAsync() > 0;
    }
}