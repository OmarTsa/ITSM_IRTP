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

        public async Task<List<Ticket>> ObtenerMisTickets(int idUsuario)
        {
            return await _http.GetFromJsonAsync<List<Ticket>>($"api/ticket/usuario/{idUsuario}") ?? new List<Ticket>();
        }

        public async Task<Ticket?> ObtenerTicket(int id)
        {
            return await _http.GetFromJsonAsync<Ticket>($"api/ticket/{id}");
        }

        public async Task GuardarTicket(Ticket ticket)
        {
            var respuesta = await _http.PostAsJsonAsync("api/ticket", ticket);
            if (!respuesta.IsSuccessStatusCode)
            {
                var error = await respuesta.Content.ReadAsStringAsync();
                throw new Exception($"Error del servidor: {error}");
            }
        }

        // --- NUEVO MÉTODO ---
        public async Task CambiarEstado(int idTicket, int nuevoEstado, int idUsuario, string? notas = "")
        {
            var request = new { IdTicket = idTicket, NuevoEstado = nuevoEstado, IdUsuario = idUsuario, Notas = notas };
            var respuesta = await _http.PostAsJsonAsync("api/ticket/cambiar-estado", request);

            if (!respuesta.IsSuccessStatusCode)
            {
                throw new Exception("No se pudo cambiar el estado del ticket.");
            }
        }

        // --- Combos ---
        public async Task<List<Categoria>> ObtenerCategorias() =>
            await _http.GetFromJsonAsync<List<Categoria>>("api/ticket/categorias") ?? new List<Categoria>();

        public async Task<List<Prioridad>> ObtenerPrioridades() =>
            await _http.GetFromJsonAsync<List<Prioridad>>("api/ticket/prioridades") ?? new List<Prioridad>();
    }
}