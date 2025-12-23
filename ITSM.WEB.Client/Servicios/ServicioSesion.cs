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

        public async Task GuardarSesion(Usuario usuario)
        {
            // SEGURIDAD: No guardamos todo el objeto Usuario (evitamos datos sensibles)
            var datosSesion = new
            {
                usuario.IdUsuario,
                usuario.Username,
                usuario.Email,
                RolNombre = usuario.Rol?.Nombre ?? "USUARIO_FINAL"
            };

            var sesionJson = JsonSerializer.Serialize(datosSesion);
            await _js.InvokeVoidAsync("localStorage.setItem", KeySesion, sesionJson);
        }

        public async Task<Usuario?> ObtenerSesion()
        {
            var sesionJson = await _js.InvokeAsync<string?>("localStorage.getItem", KeySesion);

            if (string.IsNullOrEmpty(sesionJson))
                return null;

            try
            {
                // Reconstruimos un objeto Usuario mínimo para el estado de la App
                using var doc = JsonDocument.Parse(sesionJson);
                var root = doc.RootElement;

                return new Usuario
                {
                    IdUsuario = root.GetProperty("IdUsuario").GetInt32(),
                    Username = root.GetProperty("Username").GetString(),
                    Email = root.GetProperty("Email").GetString(),
                    Rol = new Rol { Nombre = root.GetProperty("RolNombre").GetString() ?? "USUARIO_FINAL" }
                };
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