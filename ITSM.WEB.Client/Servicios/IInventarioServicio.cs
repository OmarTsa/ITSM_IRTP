using ITSM.Entidades;

namespace ITSM.WEB.Client.Servicios
{
    public interface IInventarioServicio
    {
        // Activos
        Task<List<Activo>> ListarActivos();
        Task<Activo?> ObtenerActivo(int idActivo);
        Task<Activo?> ObtenerPorId(int idActivo); // Alias
        Task RegistrarActivo(Activo activo);
        Task GuardarActivo(Activo activo); // Alias
        Task ActualizarActivo(Activo activo);

        // Tipos de Activo
        Task<List<TipoActivo>> ListarTiposActivo();
        Task<List<TipoActivo>> ListarTipos(); // Alias
        Task GuardarTipo(TipoActivo tipo);
        Task EliminarTipo(int idTipo);

        // Estados de Activo
        Task<List<EstadoActivo>> ListarEstados();
    }
}
