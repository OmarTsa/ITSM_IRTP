using Blazored.LocalStorage;
using ITSM.Entidades.DTOs;
using System.Net.Http.Headers;

namespace ITSM.WEB.Client.Auth
{
    public class ManejadorAutorizacionPersonalizado : DelegatingHandler
    {
        private readonly ILocalStorageService _almacenamientoLocal;

        public ManejadorAutorizacionPersonalizado(ILocalStorageService almacenamientoLocal)
        {
            _almacenamientoLocal = almacenamientoLocal;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage solicitud,
            CancellationToken tokenCancelacion)
        {
            try
            {
                var sesion = await _almacenamientoLocal.GetItemAsync<SesionDto>("sesionUsuario");
                
                if (sesion != null && !string.IsNullOrEmpty(sesion.Token))
                {
                    solicitud.Headers.Authorization = new AuthenticationHeaderValue("Bearer", sesion.Token);
                    Console.WriteLine($"🔐 JWT agregado a: {solicitud.Method} {solicitud.RequestUri}");
                }
                else
                {
                    Console.WriteLine($"⚠️ NO HAY TOKEN para: {solicitud.Method} {solicitud.RequestUri}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error token: {ex.Message}");
            }

            var respuesta = await base.SendAsync(solicitud, tokenCancelacion);

            if (!respuesta.IsSuccessStatusCode)
            {
                Console.WriteLine($"❌ {(int)respuesta.StatusCode} - {solicitud.RequestUri}");
            }

            return respuesta;
        }
    }
}
