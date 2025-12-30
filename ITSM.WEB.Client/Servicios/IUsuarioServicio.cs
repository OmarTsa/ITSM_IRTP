using ITSM.Entidades;
using ITSM.Entidades.DTOs;

namespace ITSM.WEB.Client.Servicios
{
    /// <summary>
    /// Interfaz para el servicio de gestión de usuarios
    /// </summary>
    public interface IUsuarioServicio
    {
        Task<List<Usuario>> Listar();
        Task<Usuario?> Obtener(int id);
        Task<bool> Guardar(Usuario modelo);
        Task<bool> Editar(Usuario modelo);
        Task<bool> Eliminar(int id);
        Task<List<Rol>> ListarRoles();
        Task<List<Area>> ListarAreas();
    }
}
