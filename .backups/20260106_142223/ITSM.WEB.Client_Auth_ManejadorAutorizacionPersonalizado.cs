using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;
using ITSM.Entidades.DTOs;
using System.Net.Http.Headers;

namespace ITSM.WEB.Client.Auth
{
    /// <summary>
    /// Manejador que intercepta todas las peticiones HTTP y agrega el JWT automáticamente
    /// </summary>
    public class ManejadorAutorizacionPersonalizado : DelegatingHandler
    {
        private readonly ILocalStorageService _almacenamientoLocal;
        private readonly NavigationManager _navegacion;

        public ManejadorAutorizacionPersonalizado(
            ILocalStorageService almacenamientoLocal,
            NavigationManager navegacion)
        {
            _almacenamientoLocal = almacenamientoLocal;
            _navegacion = navegacion;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage solicitud,
            CancellationToken tokenCancelacion)
        {
            try
            {
                // Intentar obtener la sesión del almacenamiento local
                var sesion = await _almacenamientoLocal.GetItemAsync<SesionDto>("sesionUsuario");

                if (sesion != null && !string.IsNullOrEmpty(sesion.Token))
                {
                    // Agregar el token JWT al encabezado de autorización
                    solicitud.Headers.Authorization = new AuthenticationHeaderValue("Bearer", sesion.Token);
                    Console.WriteLine($"🔐 JWT agregado a la petición: {solicitud.RequestUri}");
                }
                else
                {
                    Console.WriteLine("⚠️ No hay token JWT disponible");
                }
            }
            catch (InvalidOperationException)
            {
                // Almacenamiento local no disponible (prerendering en servidor)
                Console.WriteLine("⚠️ Almacenamiento local no disponible (prerendering)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener token: {ex.Message}");
            }

            // Continuar con la petición HTTP
            var respuesta = await base.SendAsync(solicitud, tokenCancelacion);

            // Si obtenemos 401 No Autorizado, redirigir al login
            if (respuesta.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("❌ 401 No Autorizado - Redirigiendo a login");
                _navegacion.NavigateTo("/login", forceLoad: true);
            }

            return respuesta;
        }
    }
}
