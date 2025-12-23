using ITSM.Entidades;
using System.Net.Http.Json;

namespace ITSM.WEB.Client.Servicios
{
    public class UsuarioServicio
    {
        private readonly HttpClient _http;

        public UsuarioServicio(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Usuario>> ListarUsuarios()
        {
            var respuesta = await _http.GetFromJsonAsync<List<Usuario>>("api/usuario");
            return respuesta ?? new List<Usuario>();
        }

        public async Task<Usuario?> ObtenerPorId(int id)
        {
            return await _http.GetFromJsonAsync<Usuario>($"api/usuario/{id}");
        }

        public async Task GuardarUsuario(Usuario usuario)
        {
            if (usuario.IdUsuario == 0)
                await _http.PostAsJsonAsync("api/usuario", usuario);
            else
                await _http.PutAsJsonAsync($"api/usuario/{usuario.IdUsuario}", usuario);
        }
    }
}