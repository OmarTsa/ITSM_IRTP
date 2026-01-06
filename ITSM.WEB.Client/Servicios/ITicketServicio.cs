using ITSM.Entidades;
using ITSM.Entidades.DTOs;

namespace ITSM.WEB.Client.Servicios
{
    /// <summary>
    /// Interfaz para el servicio de gestión de tickets
    /// </summary>
    public interface ITicketServicio
    {
        Task<List<Ticket>> ListarTickets();
        Task<List<Ticket>> ListarMisTickets(int idUsuario);
        Task<Ticket?> ObtenerTicket(int idTicket);
        Task<Ticket?> RegistrarTicket(Ticket ticket); // ? MODIFICADO: Ahora retorna el ticket creado
        Task ActualizarTicket(Ticket ticket);
        Task AsignarTicket(int idTicket, int idEspecialista);
        Task<List<TicketComentario>> ListarDetalles(int idTicket);
        Task AgregarComentario(TicketComentario detalle);
        Task<List<Categoria>> ListarCategorias();
        Task<List<Prioridad>> ListarPrioridades();
        Task<List<EstadoTicket>> ListarEstados();
        Task<DashboardKpi?> ObtenerKpi();
    }
}
