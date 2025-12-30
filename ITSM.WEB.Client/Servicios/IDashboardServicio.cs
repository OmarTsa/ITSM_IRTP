using ITSM.Entidades.DTOs;

namespace ITSM.WEB.Client.Servicios
{
    public interface IDashboardServicio
    {
        Task<DashboardKpi?> ObtenerKpi();
    }
}
