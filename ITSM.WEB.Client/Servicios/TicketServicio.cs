using ITSM.Entidades;
using System.Net.Http.Json;

namespace ITSM.WEB.Client.Servicios
{
    public class TicketServicio
    {
        private readonly HttpClient _http;

        public TicketServicio(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Ticket>> ObtenerTickets()
        {
            var respuesta = await _http.GetFromJsonAsync<List<Ticket>>("api/ticket");
            return respuesta ?? new List<Ticket>();
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
            var respuesta = await _http.GetFromJsonAsync<List<Ticket>>($"api/ticket/usuario/{idUsuario}");
            return respuesta ?? new List<Ticket>();
        }

        public async Task<List<Categoria>> ObtenerCategorias()
        {
            var respuesta = await _http.GetFromJsonAsync<List<Categoria>>("api/ticket/categorias");
            return respuesta ?? new List<Categoria>();
        }

        // NUEVO: MÉTODO DASHBOARD
        public async Task<DashboardKpi> ObtenerDashboard(int idUsuario)
        {
            try
            {
                var resultado = await _http.GetFromJsonAsync<DashboardKpi>($"api/ticket/kpis/{idUsuario}");
                return resultado ?? new DashboardKpi();
            }
            catch
            {
                return new DashboardKpi(); // Retornamos vacíos si falla para no romper la pantalla
            }
        }
    }
}