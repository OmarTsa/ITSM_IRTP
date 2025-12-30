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
        Task RegistrarTicket(Ticket ticket);
        Task ActualizarTicket(Ticket ticket);
        Task AsignarTicket(int idTicket, int idEspecialista);
        Task<List<TicketDetalle>> ListarDetalles(int idTicket);
        Task AgregarComentario(TicketDetalle detalle);
        Task<List<Categoria>> ListarCategorias();
        Task<List<Prioridad>> ListarPrioridades();
        Task<List<EstadoTicket>> ListarEstados();
        Task<DashboardKpi?> ObtenerKpi();
    }
}
