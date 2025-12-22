using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("GLPI_TICKETS_DETALLE")] // Asegúrate de crear esta tabla en tu BD si no existe, o deja que EF la cree si usas Migrations
    public class TicketDetalle
    {
        [Key]
        [Column("ID_DETALLE")]
        public int IdDetalle { get; set; }

        [Column("ID_TICKET")]
        public int IdTicket { get; set; }

        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario? Usuario { get; set; }

        [Column("MENSAJE")]
        public string Mensaje { get; set; } = string.Empty;

        [Column("FECHA_REGISTRO")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // 1=Comentario, 2=Cambio Estado, 3=Asignación
        [Column("TIPO_ACCION")]
        public int TipoAccion { get; set; } = 1;
    }
}