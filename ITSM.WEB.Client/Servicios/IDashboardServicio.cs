using ITSM.Entidades.DTOs;
using ITSM.WEB.Client.Servicios;



namespace ITSM.WEB.Client.Servicios
{
    public interface IDashboardServicio

    {

        Task<DashboardKpi?> ObtenerKpi();
    }


}
