using ITSM.Entidades.DTOs;

namespace ITSM.WEB.Client.Servicios
{
    /// <summary>
    /// Interfaz para el servicio de gestión de sesiones de usuario
    /// </summary>
    public interface IServicioSesion
    {
        /// <summary>
        /// Inicia sesión con las credenciales proporcionadas
        /// </summary>
        Task<SesionDto?> IniciarSesion(LoginDto credenciales);

        /// <summary>
        /// Cierra la sesión actual del usuario
        /// </summary>
        Task CerrarSesion();

        /// <summary>
        /// Obtiene la sesión actual almacenada
        /// </summary>
        Task<SesionDto?> ObtenerSesionActual();
    }
}
