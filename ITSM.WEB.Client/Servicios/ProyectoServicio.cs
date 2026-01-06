using System.Net.Http.Json;
using ITSM.Entidades;

namespace ITSM.WEB.Client.Servicios
{
    /// <summary>
    /// Servicio para gestión de proyectos del sistema ITSM
    /// </summary>
    public class ProyectoServicio : IProyectoServicio
    {
        private readonly HttpClient _clienteHttp;

        public ProyectoServicio(HttpClient clienteHttp)
        {
            _clienteHttp = clienteHttp;
        }

        // ===================================================================
        // PROYECTOS
        // ===================================================================

        public async Task<List<Proyecto>> ListarProyectos()
        {
            try
            {
                var proyectos = await _clienteHttp.GetFromJsonAsync<List<Proyecto>>("api/proyecto");
                return proyectos ?? new List<Proyecto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al listar proyectos: {ex.Message}");
                return new List<Proyecto>();
            }
        }

        public async Task<Proyecto?> ObtenerProyecto(int idProyecto)
        {
            try
            {
                return await _clienteHttp.GetFromJsonAsync<Proyecto>($"api/proyecto/{idProyecto}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener proyecto: {ex.Message}");
                return null;
            }
        }

        public async Task<Proyecto?> RegistrarProyecto(Proyecto proyecto)
        {
            try
            {
                var respuesta = await _clienteHttp.PostAsJsonAsync("api/proyecto", proyecto);
                respuesta.EnsureSuccessStatusCode();

                var proyectoCreado = await respuesta.Content.ReadFromJsonAsync<Proyecto>();
                Console.WriteLine("✅ Proyecto registrado correctamente");
                return proyectoCreado;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al registrar proyecto: {ex.Message}");
                throw;
            }
        }

        public async Task ActualizarProyecto(Proyecto proyecto)
        {
            try
            {
                var respuesta = await _clienteHttp.PutAsJsonAsync($"api/proyecto/{proyecto.IdProyecto}", proyecto);
                respuesta.EnsureSuccessStatusCode();
                Console.WriteLine("✅ Proyecto actualizado correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al actualizar proyecto: {ex.Message}");
                throw;
            }
        }

        public async Task EliminarProyecto(int idProyecto)
        {
            try
            {
                var respuesta = await _clienteHttp.DeleteAsync($"api/proyecto/{idProyecto}");
                respuesta.EnsureSuccessStatusCode();
                Console.WriteLine("✅ Proyecto eliminado correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al eliminar proyecto: {ex.Message}");
                throw;
            }
        }

        // ===================================================================
        // HITOS
        // ===================================================================

        public async Task<List<Hito>> ListarHitos(int idProyecto)
        {
            try
            {
                var hitos = await _clienteHttp.GetFromJsonAsync<List<Hito>>($"api/proyecto/{idProyecto}/hitos");
                return hitos ?? new List<Hito>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al listar hitos: {ex.Message}");
                return new List<Hito>();
            }
        }

        public async Task RegistrarHito(Hito hito)
        {
            try
            {
                var respuesta = await _clienteHttp.PostAsJsonAsync("api/proyecto/hitos", hito);
                respuesta.EnsureSuccessStatusCode();
                Console.WriteLine("✅ Hito registrado correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al registrar hito: {ex.Message}");
                throw;
            }
        }

        public async Task ActualizarHito(Hito hito)
        {
            try
            {
                var respuesta = await _clienteHttp.PutAsJsonAsync($"api/proyecto/hitos/{hito.IdHito}", hito);
                respuesta.EnsureSuccessStatusCode();
                Console.WriteLine("✅ Hito actualizado correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al actualizar hito: {ex.Message}");
                throw;
            }
        }

        public async Task EliminarHito(int idHito)
        {
            try
            {
                var respuesta = await _clienteHttp.DeleteAsync($"api/proyecto/hitos/{idHito}");
                respuesta.EnsureSuccessStatusCode();
                Console.WriteLine("✅ Hito eliminado correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al eliminar hito: {ex.Message}");
                throw;
            }
        }

        // ===================================================================
        // ENTREGABLES
        // ===================================================================

        public async Task<List<Entregable>> ListarEntregables(int idProyecto)
        {
            try
            {
                var entregables = await _clienteHttp.GetFromJsonAsync<List<Entregable>>($"api/proyecto/{idProyecto}/entregables");
                return entregables ?? new List<Entregable>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al listar entregables: {ex.Message}");
                return new List<Entregable>();
            }
        }

        public async Task RegistrarEntregable(Entregable entregable)
        {
            try
            {
                var respuesta = await _clienteHttp.PostAsJsonAsync("api/proyecto/entregables", entregable);
                respuesta.EnsureSuccessStatusCode();
                Console.WriteLine("✅ Entregable registrado correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al registrar entregable: {ex.Message}");
                throw;
            }
        }

        public async Task ActualizarEntregable(Entregable entregable)
        {
            try
            {
                var respuesta = await _clienteHttp.PutAsJsonAsync($"api/proyecto/entregables/{entregable.IdEntregable}", entregable);
                respuesta.EnsureSuccessStatusCode();
                Console.WriteLine("✅ Entregable actualizado correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al actualizar entregable: {ex.Message}");
                throw;
            }
        }

        public async Task EliminarEntregable(int idEntregable)
        {
            try
            {
                var respuesta = await _clienteHttp.DeleteAsync($"api/proyecto/entregables/{idEntregable}");
                respuesta.EnsureSuccessStatusCode();
                Console.WriteLine("✅ Entregable eliminado correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al eliminar entregable: {ex.Message}");
                throw;
            }
        }
    }
}
