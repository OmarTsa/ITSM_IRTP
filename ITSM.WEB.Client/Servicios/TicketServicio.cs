using System.Net.Http.Json;
using ITSM.Entidades;
using ITSM.Entidades.DTOs; // Necesario para los KPIs

namespace ITSM.WEB.Client.Servicios
{
    public class TicketServicio
    {
        private readonly HttpClient _http;

        public TicketServicio(HttpClient http)
        {
            _http = http;
        }

        // =============================================================
        // MÉTODOS DE LECTURA (GET)
        // =============================================================

        public async Task<List<Ticket>> ListarTickets()
        {
            try
            {
                var resultado = await _http.GetFromJsonAsync<List<Ticket>>("api/ticket");
                return resultado ?? new List<Ticket>();
            }
            catch
            {
                // Si falla, retornamos lista vacía para no romper la UI
                return new List<Ticket>();
            }
        }

        public async Task<List<Ticket>> ListarMisTickets()
        {
            try
            {
                // El backend identifica al usuario por el Token, no hace falta enviar ID
                var resultado = await _http.GetFromJsonAsync<List<Ticket>>("api/ticket/mis-tickets");
                return resultado ?? new List<Ticket>();
            }
            catch
            {
                return new List<Ticket>();
            }
        }

        public async Task<Ticket> ObtenerTicket(int id)
        {
            var ticket = await _http.GetFromJsonAsync<Ticket>($"api/ticket/{id}");
            if (ticket == null) throw new Exception("No se pudo obtener la información del ticket.");
            return ticket;
        }

        public async Task<DashboardKpi> ObtenerKpis()
        {
            try
            {
                var kpis = await _http.GetFromJsonAsync<DashboardKpi>("api/ticket/kpis");
                return kpis ?? new DashboardKpi();
            }
            catch
            {
                return new DashboardKpi();
            }
        }

        // =============================================================
        // MÉTODOS DE ESCRITURA (POST)
        // =============================================================

        public async Task RegistrarTicket(Ticket ticket)
        {
            var response = await _http.PostAsJsonAsync("api/ticket", ticket);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al registrar: {error}");
            }
        }

        public async Task AgregarComentario(TicketDetalle detalle)
        {
            var response = await _http.PostAsJsonAsync("api/ticket/comentario", detalle);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al comentar: {error}");
            }
        }

        // =============================================================
        // MÉTODOS AUXILIARES (COMBOS)
        // =============================================================

        public async Task<List<Categoria>> ListarCategorias()
        {
            try
            {
                var resultado = await _http.GetFromJsonAsync<List<Categoria>>("api/ticket/categorias");
                return resultado ?? new List<Categoria>();
            }
            catch
            {
                return new List<Categoria>();
            }
        }

        public async Task<List<Prioridad>> ListarPrioridades()
        {
            try
            {
                var resultado = await _http.GetFromJsonAsync<List<Prioridad>>("api/ticket/prioridades");
                return resultado ?? new List<Prioridad>();
            }
            catch
            {
                return new List<Prioridad>();
            }
        }
    }
}