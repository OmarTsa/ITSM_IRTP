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

        public async Task GuardarTicket(Ticket ticket)
        {
            var respuesta = await _http.PostAsJsonAsync("api/ticket", ticket);

            // ESTO ES CLAVE: Si hay error 500 (Base de datos), lanza excepción visible
            if (!respuesta.IsSuccessStatusCode)
            {
                var error = await respuesta.Content.ReadAsStringAsync();
                throw new Exception($"Error del servidor ({respuesta.StatusCode}): {error}");
            }
        }

        public async Task<List<Categoria>> ObtenerCategorias()
        {
            return await _http.GetFromJsonAsync<List<Categoria>>("api/ticket/categorias") ?? new List<Categoria>();
        }

        // Otros métodos
        public async Task<List<Prioridad>> ObtenerPrioridades() =>
            await _http.GetFromJsonAsync<List<Prioridad>>("api/ticket/prioridades") ?? new List<Prioridad>();
    }
}