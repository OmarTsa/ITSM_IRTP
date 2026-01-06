using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("HD_TICKET_COMENTARIOS")]
    public class TicketComentario
    {
        [Key]
        [Column("ID_COMENTARIO")]
        public int IdComentario { get; set; }

        [Column("ID_TICKET")]
        public int IdTicket { get; set; }

        [ForeignKey("IdTicket")]
        public virtual Ticket? Ticket { get; set; }

        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuario? Usuario { get; set; }

        [Column("COMENTARIO")]
        [Required]
        public string Comentario { get; set; } = string.Empty;

        [Column("FECHA_COMENTARIO")]
        public DateTime FechaComentario { get; set; } = DateTime.Now;

        [Column("ES_INTERNO")]
        public int EsInterno { get; set; } = 0;

        [Column("ES_SOLUCION")]
        public int EsSolucion { get; set; } = 0;

        [Column("EDITADO")]
        public int Editado { get; set; } = 0;

        [Column("FECHA_EDICION")]
        public DateTime? FechaEdicion { get; set; }

        [Column("ELIMINADO")]
        public int Eliminado { get; set; } = 0;
    }
}
