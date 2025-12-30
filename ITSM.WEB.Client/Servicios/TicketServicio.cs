using System.Net.Http.Json;
using ITSM.Entidades;
using ITSM.Entidades.DTOs;

namespace ITSM.WEB.Client.Servicios
{
    /// <summary>
    /// Servicio para gestión de tickets del sistema ITSM
    /// </summary>
    public class TicketServicio : ITicketServicio
    {
        private readonly HttpClient _clienteHttp;

        public TicketServicio(HttpClient clienteHttp)
        {
            _clienteHttp = clienteHttp;
        }

        public async Task<List<Ticket>> ListarTickets()
        {
            try
            {
                var tickets = await _clienteHttp.GetFromJsonAsync<List<Ticket>>("api/ticket");
                return tickets ?? new List<Ticket>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al listar tickets: {ex.Message}");
                return new List<Ticket>();
            }
        }

        public async Task<List<Ticket>> ListarMisTickets(int idUsuario)
        {
            try
            {
                var tickets = await _clienteHttp.GetFromJsonAsync<List<Ticket>>($"api/ticket/mis/{idUsuario}");
                return tickets ?? new List<Ticket>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al listar mis tickets: {ex.Message}");
                return new List<Ticket>();
            }
        }

        public async Task<Ticket?> ObtenerTicket(int idTicket)
        {
            try
            {
                return await _clienteHttp.GetFromJsonAsync<Ticket>($"api/ticket/{idTicket}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener ticket: {ex.Message}");
                return null;
            }
        }

        public async Task RegistrarTicket(Ticket ticket)
        {
            try
            {
                var respuesta = await _clienteHttp.PostAsJsonAsync("api/ticket", ticket);
                respuesta.EnsureSuccessStatusCode();
                Console.WriteLine("✅ Ticket registrado correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al registrar ticket: {ex.Message}");
                throw;
            }
        }

        public async Task ActualizarTicket(Ticket ticket)
        {
            try
            {
                var respuesta = await _clienteHttp.PutAsJsonAsync($"api/ticket/{ticket.IdTicket}", ticket);
                respuesta.EnsureSuccessStatusCode();
                Console.WriteLine("✅ Ticket actualizado correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al actualizar ticket: {ex.Message}");
                throw;
            }
        }

        public async Task AsignarTicket(int idTicket, int idEspecialista)
        {
            try
            {
                var respuesta = await _clienteHttp.PostAsync($"api/ticket/{idTicket}/asignar/{idEspecialista}", null);
                respuesta.EnsureSuccessStatusCode();
                Console.WriteLine("✅ Ticket asignado correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al asignar ticket: {ex.Message}");
                throw;
            }
        }

        public async Task<List<TicketDetalle>> ListarDetalles(int idTicket)
        {
            try
            {
                var detalles = await _clienteHttp.GetFromJsonAsync<List<TicketDetalle>>($"api/ticket/{idTicket}/detalle");
                return detalles ?? new List<TicketDetalle>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al listar detalles: {ex.Message}");
                return new List<TicketDetalle>();
            }
        }

        public async Task AgregarComentario(TicketDetalle detalle)
        {
            try
            {
                var respuesta = await _clienteHttp.PostAsJsonAsync($"api/ticket/{detalle.IdTicket}/detalle", detalle);
                respuesta.EnsureSuccessStatusCode();
                Console.WriteLine("✅ Comentario agregado correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al agregar comentario: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Categoria>> ListarCategorias()
        {
            try
            {
                var categorias = await _clienteHttp.GetFromJsonAsync<List<Categoria>>("api/ticket/categorias");
                return categorias ?? new List<Categoria>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al listar categorías: {ex.Message}");
                return new List<Categoria>();
            }
        }

        public async Task<List<Prioridad>> ListarPrioridades()
        {
            try
            {
                var prioridades = await _clienteHttp.GetFromJsonAsync<List<Prioridad>>("api/ticket/prioridades");
                return prioridades ?? new List<Prioridad>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al listar prioridades: {ex.Message}");
                return new List<Prioridad>();
            }
        }

        public async Task<List<EstadoTicket>> ListarEstados()
        {
            try
            {
                var estados = await _clienteHttp.GetFromJsonAsync<List<EstadoTicket>>("api/ticket/estados");
                return estados ?? new List<EstadoTicket>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al listar estados: {ex.Message}");
                return new List<EstadoTicket>();
            }
        }

        public async Task<DashboardKpi?> ObtenerKpi()
        {
            try
            {
                return await _clienteHttp.GetFromJsonAsync<DashboardKpi>("api/ticket/kpi");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener KPI: {ex.Message}");
                return null;
            }
        }
    }
}
