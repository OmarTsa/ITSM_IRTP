namespace ITSM.Entidades
{
    public class DashboardKpi
    {
        public int TotalTickets { get; set; }
        public int TicketsAbiertos { get; set; }
        public int TicketsResueltos { get; set; }
        public int TicketsCriticos { get; set; }
        public int MisAsignados { get; set; }
    }
}