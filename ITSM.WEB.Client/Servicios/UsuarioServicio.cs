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

        public async Task<Usuario?> Login(string email, string password)
        {
            // Enviamos un objeto anónimo con las credenciales al controlador de autenticación
            var response = await _http.PostAsJsonAsync("api/autenticacion/login", new { Email = email, Password = password });

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Usuario>();
            }
            return null;
        }

        public async Task<List<Usuario>> ListarUsuarios() =>
            await _http.GetFromJsonAsync<List<Usuario>>("api/usuario") ?? new List<Usuario>();

        public async Task<Usuario?> ObtenerPorId(int id) =>
            await _http.GetFromJsonAsync<Usuario>($"api/usuario/{id}");

        public async Task GuardarUsuario(Usuario usuario)
        {
            if (usuario.IdUsuario == 0)
                await _http.PostAsJsonAsync("api/usuario", usuario);
            else
                await _http.PutAsJsonAsync($"api/usuario/{usuario.IdUsuario}", usuario);
        }
    }
}