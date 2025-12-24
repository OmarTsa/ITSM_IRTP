using System.Net.Http.Json;
using ITSM.Entidades;

namespace ITSM.WEB.Client.Servicios
{
    public class UsuarioServicio
    {
        private readonly HttpClient _http;

        public UsuarioServicio(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Usuario>> Listar()
        {
            return await _http.GetFromJsonAsync<List<Usuario>>("api/usuario") ?? new List<Usuario>();
        }

        public async Task<Usuario> Obtener(int id)
        {
            return await _http.GetFromJsonAsync<Usuario>($"api/usuario/{id}");
        }

        public async Task Guardar(Usuario usuario)
        {
            if (usuario.IdUsuario == 0)
            {
                // Crear
                var response = await _http.PostAsJsonAsync("api/usuario", usuario);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception(error);
                }
            }
            else
            {
                // Editar
                var response = await _http.PutAsJsonAsync("api/usuario", usuario);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception(error);
                }
            }
        }

        public async Task Eliminar(int id)
        {
            var response = await _http.DeleteAsync($"api/usuario/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("No se pudo dar de baja al usuario.");
            }
        }

        public async Task<List<Rol>> ListarRoles()
        {
            return await _http.GetFromJsonAsync<List<Rol>>("api/usuario/roles") ?? new List<Rol>();
        }

        public async Task<List<Area>> ListarAreas()
        {
            return await _http.GetFromJsonAsync<List<Area>>("api/usuario/areas") ?? new List<Area>();
        }
    }
}