using System.Net.Http.Json;
using ITSM.Entidades;

namespace ITSM.WEB.Client.Servicios
{
    public class TicketServicio
    {
        private readonly HttpClient _http;

        public TicketServicio(HttpClient http)
        {
            _http = http;
        }

        // --- MÉTODOS DE LECTURA ---

        public async Task<List<Ticket>> ListarTickets()
        {
            return await _http.GetFromJsonAsync<List<Ticket>>("api/ticket") ?? new List<Ticket>();
        }

        // Este es el método que faltaba para 'MisTickets.razor'
        public async Task<List<Ticket>> ListarMisTickets()
        {
            // El backend sabrá quién es el usuario por el Token JWT, así que usamos un endpoint específico
            return await _http.GetFromJsonAsync<List<Ticket>>("api/ticket/mis-tickets") ?? new List<Ticket>();
        }

        // Este es el método que faltaba para 'DetalleTicket.razor'
        public async Task<Ticket> ObtenerTicket(int id)
        {
            var ticket = await _http.GetFromJsonAsync<Ticket>($"api/ticket/{id}");
            if (ticket == null) throw new Exception("No se pudo obtener el ticket.");
            return ticket;
        }

        // --- MÉTODOS DE ESCRITURA ---

        public async Task RegistrarTicket(Ticket ticket)
        {
            var response = await _http.PostAsJsonAsync("api/ticket", ticket);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }

        // --- MÉTODOS AUXILIARES (COMBOS) ---

        public async Task<List<Categoria>> ListarCategorias()
        {
            return await _http.GetFromJsonAsync<List<Categoria>>("api/ticket/categorias") ?? new List<Categoria>();
        }

        public async Task<List<Prioridad>> ListarPrioridades()
        {
            // Si tienes un endpoint para prioridades, úsalo. Si no, retornamos una lista vacía o hardcodeada por seguridad
            // return await _http.GetFromJsonAsync<List<Prioridad>>("api/ticket/prioridades");
            return new List<Prioridad>();
        }
    }
}