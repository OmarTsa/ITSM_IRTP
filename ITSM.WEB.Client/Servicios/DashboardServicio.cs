using System.Net.Http.Json;
using ITSM.Entidades.DTOs;

namespace ITSM.WEB.Client.Servicios
{
    public class DashboardServicio : IDashboardServicio
    {
        private readonly HttpClient _clienteHttp;

        public DashboardServicio(HttpClient clienteHttp)
        {
            _clienteHttp = clienteHttp;
        }

        public async Task<DashboardKpi?> ObtenerKpi()
        {
            try
            {
                return await _clienteHttp.GetFromJsonAsync<DashboardKpi>("api/dashboard/kpi");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener KPI del dashboard: {ex.Message}");
                return null;
            }
        }
    }
}
