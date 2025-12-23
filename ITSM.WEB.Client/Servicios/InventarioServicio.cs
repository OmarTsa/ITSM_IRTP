using ITSM.Entidades;
using System.Net.Http.Json;

namespace ITSM.WEB.Client.Servicios
{
    public class InventarioServicio
    {
        private readonly HttpClient _http;

        public InventarioServicio(HttpClient http) { _http = http; }

        public async Task<List<Activo>> ListarActivos() =>
            await _http.GetFromJsonAsync<List<Activo>>("api/activo") ?? new List<Activo>();

        public async Task<Activo?> ObtenerActivo(int id) =>
            await _http.GetFromJsonAsync<Activo>($"api/activo/{id}");

        public async Task GuardarActivo(Activo activo)
        {
            if (activo.IdActivo == 0) await _http.PostAsJsonAsync("api/activo", activo);
            else await _http.PutAsJsonAsync($"api/activo/{activo.IdActivo}", activo);
        }

        public async Task<List<Activo>> ListarPorUsuario(int idUsuario)
        {
            return await _http.GetFromJsonAsync<List<Activo>>($"api/activo/usuario/{idUsuario}")
                   ?? new List<Activo>();
        }

        public async Task<List<TipoActivo>> ListarTipos() =>
            await _http.GetFromJsonAsync<List<TipoActivo>>("api/activo/tipos") ?? new List<TipoActivo>();

        public async Task GuardarTipo(TipoActivo tipo)
        {
            HttpResponseMessage response;
            if (tipo.IdTipo == 0) response = await _http.PostAsJsonAsync("api/activo/tipos", tipo);
            else response = await _http.PutAsJsonAsync($"api/activo/tipos/{tipo.IdTipo}", tipo);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }

        public async Task EliminarTipo(int id)
        {
            var response = await _http.DeleteAsync($"api/activo/tipos/{id}");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }
    }
}