namespace ITSM.Entidades.DTOs
{
    public class DashboardKpi
    {
        public int TotalTickets { get; set; }
        public int Pendientes { get; set; }
        public int Resueltos { get; set; }
        public int PorcentajeAtencion { get; set; }

        // Propiedades adicionales útiles para gráficos
        public int EnProceso { get; set; }
        public int Abiertos { get; set; }
    }
}