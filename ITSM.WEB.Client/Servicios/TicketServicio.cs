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

        // MODIFICADO: Ahora pide el ID del usuario
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
            await _http.PostAsJsonAsync("api/ticket", ticket);
        }

        // --- Catálogos ---

        public async Task<List<Categoria>> ObtenerCategorias()
        {
            return await _http.GetFromJsonAsync<List<Categoria>>("api/ticket/categorias") ?? new List<Categoria>();
        }

        public async Task<List<Prioridad>> ObtenerPrioridades()
        {
            return await _http.GetFromJsonAsync<List<Prioridad>>("api/ticket/prioridades") ?? new List<Prioridad>();
        }
    }
}