using ITSM.Entidades.DTOs;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using ITSM.WEB.Client.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text.Json;

namespace ITSM.WEB.Client.Servicios
{
    /// <summary>
    /// Servicio para gestionar la autenticación y sesiones de usuario
    /// </summary>
    public class ServicioSesion : IServicioSesion
    {
        private readonly HttpClient _clienteHttp;
        private readonly ILocalStorageService _almacenamientoLocal;
        private readonly AuthenticationStateProvider _proveedorEstadoAutenticacion;
        private const string CLAVE_SESION = "sesionUsuario";

        public ServicioSesion(
            HttpClient clienteHttp,
            ILocalStorageService almacenamientoLocal,
            AuthenticationStateProvider proveedorEstadoAutenticacion)
        {
            _clienteHttp = clienteHttp;
            _almacenamientoLocal = almacenamientoLocal;
            _proveedorEstadoAutenticacion = proveedorEstadoAutenticacion;
        }

        /// <summary>
        /// Inicia sesión con las credenciales del usuario
        /// </summary>
        public async Task<SesionDto?> IniciarSesion(LoginDto credenciales)
        {
            try
            {
                Console.WriteLine($"🔍 Iniciando sesión para: {credenciales.Username}");

                // Realizar petición de login al servidor
                var respuesta = await _clienteHttp.PostAsJsonAsync("api/autenticacion/login", credenciales);

                if (respuesta.IsSuccessStatusCode)
                {
                    var sesion = await respuesta.Content.ReadFromJsonAsync<SesionDto>();

                    if (sesion != null && !string.IsNullOrEmpty(sesion.Token))
                    {
                        // Guardar sesión en almacenamiento local
                        await _almacenamientoLocal.SetItemAsync(CLAVE_SESION, sesion);

                        // Notificar al proveedor de autenticación
                        ((ProveedorAutenticacion)_proveedorEstadoAutenticacion)
                            .NotificarUsuarioLogueado(sesion.Token);

                        Console.WriteLine($"✅ Login exitoso: {sesion.Username} - Rol: {sesion.Rol}");
                        Console.WriteLine($"🔑 Token JWT almacenado correctamente");

                        return sesion;
                    }
                    else
                    {
                        Console.WriteLine("❌ Respuesta del servidor sin datos de sesión válidos");
                    }
                }
                else
                {
                    var mensajeError = await respuesta.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ Login fallido: {respuesta.StatusCode} - {mensajeError}");
                }

                return null;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"❌ Error de conexión al servidor: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Excepción en login: {ex.Message}");
                Console.WriteLine($"   StackTrace: {ex.StackTrace}");
                return null;
            }
        }

        /// <summary>
        /// Cierra la sesión actual del usuario
        /// </summary>
        public async Task CerrarSesion()
        {
            try
            {
                Console.WriteLine("🔐 Cerrando sesión...");

                // Eliminar sesión del almacenamiento local
                await _almacenamientoLocal.RemoveItemAsync(CLAVE_SESION);

                // Notificar al proveedor de autenticación
                ((ProveedorAutenticacion)_proveedorEstadoAutenticacion)
                    .NotificarUsuarioDeslogueado();

                Console.WriteLine("✅ Sesión cerrada correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al cerrar sesión: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene la sesión actual almacenada en localStorage
        /// </summary>
        public async Task<SesionDto?> ObtenerSesionActual()
        {
            try
            {
                var sesion = await _almacenamientoLocal.GetItemAsync<SesionDto>(CLAVE_SESION);

                if (sesion != null)
                {
                    Console.WriteLine($"✅ Sesión encontrada: {sesion.Username}");
                }
                else
                {
                    Console.WriteLine("⚠️ No hay sesión activa");
                }

                return sesion;
            }
            catch (JsonException ex)
            {
                // ⚠️ DATOS CORRUPTOS EN LOCALSTORAGE - LIMPIEZA AUTOMÁTICA
                Console.WriteLine($"⚠️ JSON corrupto detectado en localStorage: {ex.Message}");
                Console.WriteLine($"🧹 Limpiando datos corruptos automáticamente...");

                try
                {
                    await _almacenamientoLocal.RemoveItemAsync(CLAVE_SESION);
                    Console.WriteLine("✅ LocalStorage limpio. Por favor, inicia sesión nuevamente.");
                }
                catch
                {
                    Console.WriteLine("⚠️ No se pudo limpiar localStorage automáticamente");
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener sesión: {ex.Message}");
                return null;
            }
        }
    }
}
