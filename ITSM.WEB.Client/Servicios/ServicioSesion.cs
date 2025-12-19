using Microsoft.JSInterop;
using System.Text.Json;
using ITSM.Entidades;

namespace ITSM.WEB.Client.Servicios
{
    public class ServicioSesion
    {
        private readonly IJSRuntime _js;

        public ServicioSesion(IJSRuntime js)
        {
            _js = js;
        }

        public async Task GuardarUsuario(Usuario usuario)
        {
            try
            {
                var json = JsonSerializer.Serialize(usuario);
                await _js.InvokeVoidAsync("localStorage.setItem", "usuario_sesion", json);
            }
            catch
            {
                // Ignorar errores (ej. prerenderizado)
            }
        }

        public async Task<Usuario?> ObtenerUsuario()
        {
            try
            {
                var json = await _js.InvokeAsync<string>("localStorage.getItem", "usuario_sesion");

                if (string.IsNullOrEmpty(json)) return null;

                return JsonSerializer.Deserialize<Usuario>(json);
            }
            catch (Exception) // <--- CAMBIO IMPORTANTE: Captura CUALQUIER error (JSON corrupto, etc.)
            {
                // Si hay un error leyendo la sesión (datos corruptos), la limpiamos y retornamos null
                await CerrarSesion();
                return null;
            }
        }

        public async Task CerrarSesion()
        {
            try
            {
                await _js.InvokeVoidAsync("localStorage.removeItem", "usuario_sesion");
            }
            catch
            {
                // Ignorar
            }
        }
    }
}