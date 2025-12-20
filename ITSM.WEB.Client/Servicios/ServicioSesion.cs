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

        public async Task<Usuario?> ObtenerUsuario()
        {
            try
            {
                var json = await _js.InvokeAsync<string>("localStorage.getItem", "usuario_sesion");
                if (string.IsNullOrEmpty(json)) return null;
                return JsonSerializer.Deserialize<Usuario>(json);
            }
            catch
            {
                return null;
            }
        }

        public async Task GuardarUsuario(Usuario usuario)
        {
            var json = JsonSerializer.Serialize(usuario);
            await _js.InvokeVoidAsync("localStorage.setItem", "usuario_sesion", json);
        }

        public async Task CerrarSesion()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", "usuario_sesion");
        }
    }
}