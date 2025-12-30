using System.Net.Http.Json;
using ITSM.Entidades;

namespace ITSM.WEB.Client.Servicios
{
    public class InventarioServicio : IInventarioServicio
    {
        private readonly HttpClient _clienteHttp;

        public InventarioServicio(HttpClient clienteHttp)
        {
            _clienteHttp = clienteHttp;
        }

        // ===== ACTIVOS =====
        public async Task<List<Activo>> ListarActivos()
        {
            try
            {
                var activos = await _clienteHttp.GetFromJsonAsync<List<Activo>>("api/activo");
                return activos ?? new List<Activo>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al listar activos: {ex.Message}");
                return new List<Activo>();
            }
        }

        public async Task<Activo?> ObtenerActivo(int idActivo)
        {
            try
            {
                return await _clienteHttp.GetFromJsonAsync<Activo>($"api/activo/{idActivo}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener activo: {ex.Message}");
                return null;
            }
        }

        public async Task<Activo?> ObtenerPorId(int idActivo)
        {
            return await ObtenerActivo(idActivo);
        }

        public async Task RegistrarActivo(Activo activo)
        {
            try
            {
                var respuesta = await _clienteHttp.PostAsJsonAsync("api/activo", activo);
                respuesta.EnsureSuccessStatusCode();
                Console.WriteLine("✅ Activo registrado correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al registrar activo: {ex.Message}");
                throw;
            }
        }

        public async Task GuardarActivo(Activo activo)
        {
            if (activo.IdActivo > 0)
            {
                await ActualizarActivo(activo);
            }
            else
            {
                await RegistrarActivo(activo);
            }
        }

        public async Task ActualizarActivo(Activo activo)
        {
            try
            {
                var respuesta = await _clienteHttp.PutAsJsonAsync($"api/activo/{activo.IdActivo}", activo);
                respuesta.EnsureSuccessStatusCode();
                Console.WriteLine("✅ Activo actualizado correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al actualizar activo: {ex.Message}");
                throw;
            }
        }

        // ===== TIPOS DE ACTIVO =====
        public async Task<List<TipoActivo>> ListarTiposActivo()
        {
            try
            {
                var tipos = await _clienteHttp.GetFromJsonAsync<List<TipoActivo>>("api/activo/tipos");
                return tipos ?? new List<TipoActivo>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al listar tipos de activo: {ex.Message}");
                return new List<TipoActivo>();
            }
        }

        public async Task<List<TipoActivo>> ListarTipos()
        {
            return await ListarTiposActivo();
        }

        public async Task GuardarTipo(TipoActivo tipo)
        {
            try
            {
                HttpResponseMessage respuesta;

                if (tipo.IdTipoActivo > 0)
                {
                    respuesta = await _clienteHttp.PutAsJsonAsync($"api/activo/tipos/{tipo.IdTipoActivo}", tipo);
                }
                else
                {
                    respuesta = await _clienteHttp.PostAsJsonAsync("api/activo/tipos", tipo);
                }

                respuesta.EnsureSuccessStatusCode();
                Console.WriteLine("✅ Tipo de activo guardado correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al guardar tipo: {ex.Message}");
                throw;
            }
        }

        public async Task EliminarTipo(int idTipo)
        {
            try
            {
                var respuesta = await _clienteHttp.DeleteAsync($"api/activo/tipos/{idTipo}");
                respuesta.EnsureSuccessStatusCode();
                Console.WriteLine("✅ Tipo de activo eliminado correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al eliminar tipo: {ex.Message}");
                throw;
            }
        }
    }
}
