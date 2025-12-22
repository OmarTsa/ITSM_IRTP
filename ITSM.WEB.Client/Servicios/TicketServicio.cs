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

        public async Task<List<Categoria>> ObtenerCategorias()
        {
            return await _http.GetFromJsonAsync<List<Categoria>>("api/ticket/categorias") ?? new List<Categoria>();
        }

        // Método Espejo para obtener Ticket por ID
        public async Task<Ticket?> ObtenerTicketPorId(int id)
        {
            try
            {
                return await _http.GetFromJsonAsync<Ticket>($"api/ticket/{id}");
            }
            catch { return null; }
        }

        public async Task<List<Ticket>> ObtenerMisTickets(int idUsuario)
        {
            // Nota: Debes crear este endpoint en el Controller si no existe, 
            // o usar un endpoint genérico. Por ahora devolvemos lista vacía si falla para que no explote.
            try
            {
                return await _http.GetFromJsonAsync<List<Ticket>>($"api/ticket/usuario/{idUsuario}") ?? new List<Ticket>();
            }
            catch { return new List<Ticket>(); }
        }

        public async Task GuardarTicket(Ticket ticket)
        {
            var respuesta = await _http.PostAsJsonAsync("api/ticket", ticket);
            if (!respuesta.IsSuccessStatusCode) throw new Exception("Error al guardar ticket");
        }
    }
}