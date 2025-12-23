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

        public async Task<List<Ticket>> ListarTodosLosTicketsAsync() =>
            await _http.GetFromJsonAsync<List<Ticket>>("api/ticket") ?? new List<Ticket>();

        public async Task<List<Ticket>> ObtenerTicketsPorUsuario(int idUsuario) =>
            await _http.GetFromJsonAsync<List<Ticket>>($"api/ticket/usuario/{idUsuario}") ?? new List<Ticket>();

        public async Task<Ticket?> ObtenerTicketPorId(int id) =>
            await _http.GetFromJsonAsync<Ticket>($"api/ticket/{id}");

        public async Task<List<Categoria>> ListarCategorias() =>
            await _http.GetFromJsonAsync<List<Categoria>>("api/ticket/categorias") ?? new List<Categoria>();

        public async Task GuardarTicket(Ticket ticket)
        {
            if (ticket.IdTicket == 0)
                await _http.PostAsJsonAsync("api/ticket", ticket);
            else
                await _http.PutAsJsonAsync($"api/ticket/{ticket.IdTicket}", ticket);
        }
    }
}