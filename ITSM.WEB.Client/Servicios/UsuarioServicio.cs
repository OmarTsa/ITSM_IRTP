using System.Net.Http.Json;
using ITSM.Entidades;

namespace ITSM.WEB.Client.Servicios
{
    /// <summary>
    /// Servicio para gestión de usuarios del sistema
    /// </summary>
    public class UsuarioServicio : IUsuarioServicio
    {
        private readonly HttpClient _clienteHttp;

        public UsuarioServicio(HttpClient clienteHttp)
        {
            _clienteHttp = clienteHttp;
        }

        public async Task<List<Usuario>> Listar()
        {
            try
            {
                var usuarios = await _clienteHttp.GetFromJsonAsync<List<Usuario>>("api/usuario");
                return usuarios ?? new List<Usuario>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al listar usuarios: {ex.Message}");
                return new List<Usuario>();
            }
        }

        public async Task<Usuario?> Obtener(int id)
        {
            try
            {
                return await _clienteHttp.GetFromJsonAsync<Usuario>($"api/usuario/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener usuario: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> Guardar(Usuario modelo)
        {
            try
            {
                var respuesta = await _clienteHttp.PostAsJsonAsync("api/usuario", modelo);
                var exitoso = respuesta.IsSuccessStatusCode;

                if (exitoso)
                    Console.WriteLine("✅ Usuario guardado correctamente");
                else
                    Console.WriteLine($"❌ Error al guardar usuario: {respuesta.StatusCode}");

                return exitoso;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Excepción al guardar usuario: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> Editar(Usuario modelo)
        {
            try
            {
                var respuesta = await _clienteHttp.PutAsJsonAsync("api/usuario", modelo);
                var exitoso = respuesta.IsSuccessStatusCode;

                if (exitoso)
                    Console.WriteLine("✅ Usuario editado correctamente");
                else
                    Console.WriteLine($"❌ Error al editar usuario: {respuesta.StatusCode}");

                return exitoso;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Excepción al editar usuario: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var respuesta = await _clienteHttp.DeleteAsync($"api/usuario/{id}");
                var exitoso = respuesta.IsSuccessStatusCode;

                if (exitoso)
                    Console.WriteLine("✅ Usuario eliminado correctamente");
                else
                    Console.WriteLine($"❌ Error al eliminar usuario: {respuesta.StatusCode}");

                return exitoso;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Excepción al eliminar usuario: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Rol>> ListarRoles()
        {
            try
            {
                var roles = await _clienteHttp.GetFromJsonAsync<List<Rol>>("api/usuario/roles");
                return roles ?? new List<Rol>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al listar roles: {ex.Message}");
                return new List<Rol>();
            }
        }

        public async Task<List<Area>> ListarAreas()
        {
            try
            {
                var areas = await _clienteHttp.GetFromJsonAsync<List<Area>>("api/usuario/areas");
                return areas ?? new List<Area>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al listar áreas: {ex.Message}");
                return new List<Area>();
            }
        }
    }
}
