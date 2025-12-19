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
            // Aquí también protegemos por si acaso, aunque el login suele ser interactivo
            try
            {
                var json = JsonSerializer.Serialize(usuario);
                await _js.InvokeVoidAsync("localStorage.setItem", "usuario_sesion", json);
            }
            catch
            {
                // Ignorar errores en pre-renderizado
            }
        }

        public async Task<Usuario?> ObtenerUsuario()
        {
            try
            {
                // Intentamos leer del navegador
                var json = await _js.InvokeAsync<string>("localStorage.getItem", "usuario_sesion");

                if (string.IsNullOrEmpty(json)) return null;

                return JsonSerializer.Deserialize<Usuario>(json);
            }
            catch (InvalidOperationException)
            {
                // ESTO ES LA SOLUCIÓN:
                // Si atrapamos este error, significa que estamos en el Servidor (Pre-renderizado).
                // Simplemente retornamos null, como si no hubiera usuario logueado todavía.
                return null;
            }
            catch (JSException)
            {
                // Si hay un error de Javascript, asumimos que no hay sesión
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
                // Ignorar si falla
            }
        }
    }
}