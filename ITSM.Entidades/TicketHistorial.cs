using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("HD_TICKET_HISTORIAL")]
    public class TicketHistorial
    {
        [Key]
        [Column("ID_HISTORIAL")]
        public int IdHistorial { get; set; }

        [Column("ID_TICKET")]
        public int IdTicket { get; set; }

        [ForeignKey("IdTicket")]
        public virtual Ticket? Ticket { get; set; }

        [Column("ID_ESTADO_ANTERIOR")]
        public int? IdEstadoAnterior { get; set; }

        [ForeignKey("IdEstadoAnterior")]
        public virtual EstadoTicket? EstadoAnterior { get; set; }

        [Column("ID_ESTADO_NUEVO")]
        public int IdEstadoNuevo { get; set; }

        [ForeignKey("IdEstadoNuevo")]
        public virtual EstadoTicket? EstadoNuevo { get; set; }

        [Column("ID_USUARIO_CAMBIO")]
        public int IdUsuarioCambio { get; set; }

        [ForeignKey("IdUsuarioCambio")]
        public virtual Usuario? UsuarioCambio { get; set; }

        [Column("FECHA_CAMBIO")]
        public DateTime FechaCambio { get; set; } = DateTime.Now;

        [Column("COMENTARIO")]
        [StringLength(500)]
        public string? Comentario { get; set; }
    }
}
