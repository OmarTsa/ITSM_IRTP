using ITSM.Entidades;

namespace ITSM.WEB.Client.Servicios
{
    /// <summary>
    /// Interfaz para el servicio de gestión de proyectos
    /// </summary>
    public interface IProyectoServicio
    {
        // Proyectos
        Task<List<Proyecto>> ListarProyectos();
        Task<Proyecto?> ObtenerProyecto(int idProyecto);
        Task<Proyecto?> RegistrarProyecto(Proyecto proyecto);
        Task ActualizarProyecto(Proyecto proyecto);
        Task EliminarProyecto(int idProyecto);

        // Hitos
        Task<List<Hito>> ListarHitos(int idProyecto);
        Task RegistrarHito(Hito hito);
        Task ActualizarHito(Hito hito);
        Task EliminarHito(int idHito);

        // Entregables
        Task<List<Entregable>> ListarEntregables(int idProyecto);
        Task RegistrarEntregable(Entregable entregable);
        Task ActualizarEntregable(Entregable entregable);
        Task EliminarEntregable(int idEntregable);
    }
}
