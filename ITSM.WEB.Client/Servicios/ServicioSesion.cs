using Microsoft.JSInterop;
using ITSM.Entidades;
using System.Text.Json;

namespace ITSM.WEB.Client.Servicios
{
    public class ServicioSesion
    {
        private readonly IJSRuntime _js;
        private const string KeySesion = "sesion_usuario";

        public ServicioSesion(IJSRuntime js)
        {
            _js = js;
        }

        public async Task GuardarSesion(Usuario sesion)
        {
            var sesionJson = JsonSerializer.Serialize(sesion);
            await _js.InvokeVoidAsync("localStorage.setItem", KeySesion, sesionJson);
        }

        public async Task<Usuario?> ObtenerSesion()
        {
            var sesionJson = await _js.InvokeAsync<string?>("localStorage.getItem", KeySesion);

            if (string.IsNullOrEmpty(sesionJson))
                return null;

            try
            {
                return JsonSerializer.Deserialize<Usuario>(sesionJson);
            }
            catch
            {
                return null;
            }
        }

        public async Task LimpiarSesion()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", KeySesion);
        }
    }
}