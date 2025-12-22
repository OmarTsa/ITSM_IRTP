using ITSM.Entidades;
using System.Net.Http.Json;

namespace ITSM.WEB.Client.Servicios
{
    public class TicketServicio
    {
        private readonly HttpClient _http;

        public TicketServicio(HttpClient http) { _http = http; }

        public async Task<List<Ticket>> ObtenerTickets()
        {
            var r = await _http.GetFromJsonAsync<List<Ticket>>("api/ticket");
            return r ?? new List<Ticket>();
        }

        public async Task<Ticket?> ObtenerTicketPorId(int id)
        {
            return await _http.GetFromJsonAsync<Ticket>($"api/ticket/{id}");
        }

        public async Task GuardarTicket(Ticket ticket)
        {
            if (ticket.IdTicket == 0)
                await _http.PostAsJsonAsync("api/ticket", ticket);
            else
                await _http.PutAsJsonAsync($"api/ticket/{ticket.IdTicket}", ticket);
        }

        public async Task<List<Ticket>> ListarPorUsuario(int idUsuario)
        {
            var r = await _http.GetFromJsonAsync<List<Ticket>>($"api/ticket/usuario/{idUsuario}");
            return r ?? new List<Ticket>();
        }

        public async Task<List<Categoria>> ObtenerCategorias() =>
            await _http.GetFromJsonAsync<List<Categoria>>("api/ticket/categorias") ?? new List<Categoria>();

        public async Task<List<Prioridad>> ObtenerPrioridades() =>
            await _http.GetFromJsonAsync<List<Prioridad>>("api/ticket/prioridades") ?? new List<Prioridad>();

        public async Task<List<Estado>> ObtenerEstados() =>
            await _http.GetFromJsonAsync<List<Estado>>("api/ticket/estados") ?? new List<Estado>();

        public async Task<List<Usuario>> ObtenerEspecialistas() =>
            await _http.GetFromJsonAsync<List<Usuario>>("api/ticket/especialistas") ?? new List<Usuario>();

        // --- ESTO SOLUCIONA LOS ERRORES DE MÉTODOS FALTANTES ---
        public async Task<List<Activo>> ObtenerActivos() =>
            await _http.GetFromJsonAsync<List<Activo>>("api/ticket/activos") ?? new List<Activo>();

        public async Task<List<Usuario>> ObtenerTodosUsuarios() =>
            await _http.GetFromJsonAsync<List<Usuario>>("api/ticket/usuarios-lista") ?? new List<Usuario>();
        // -------------------------------------------------------

        public async Task<DashboardKpi> ObtenerDashboard(int idUsuario)
        {
            try { return await _http.GetFromJsonAsync<DashboardKpi>($"api/ticket/kpis/{idUsuario}") ?? new DashboardKpi(); }
            catch { return new DashboardKpi(); }
        }

        public async Task<List<TicketDetalle>> ObtenerDetalles(int idTicket) =>
            await _http.GetFromJsonAsync<List<TicketDetalle>>($"api/ticket/{idTicket}/detalles") ?? new List<TicketDetalle>();

        public async Task GuardarComentario(TicketDetalle detalle)
        {
            await _http.PostAsJsonAsync("api/ticket/comentario", detalle);
        }
    }
}