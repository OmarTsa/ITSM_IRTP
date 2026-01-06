using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("HD_TICKET_ADJUNTOS")]
    public class TicketAdjunto
    {
        [Key]
        [Column("ID_ADJUNTO")]
        public int IdAdjunto { get; set; }

        [Column("ID_TICKET")]
        public int IdTicket { get; set; }

        [ForeignKey("IdTicket")]
        public virtual Ticket? Ticket { get; set; }

        [Column("NOMBRE_ARCHIVO")]
        [Required]
        [StringLength(255)]
        public string NombreArchivo { get; set; } = string.Empty;

        [Column("NOMBRE_ORIGINAL")]
        [StringLength(255)]
        public string? NombreOriginal { get; set; }

        [Column("RUTA_ARCHIVO")]
        [StringLength(500)]
        public string? RutaArchivo { get; set; }

        [Column("MIME_TYPE")]
        [StringLength(100)]
        public string? MimeType { get; set; }

        [Column("TAMAÑO_BYTES")]
        public int? TamañoBytes { get; set; }

        [Column("HASH_ARCHIVO")]
        [StringLength(256)]
        public string? HashArchivo { get; set; }

        [Column("ID_USUARIO_SUBIDA")]
        public int IdUsuarioSubida { get; set; }

        [ForeignKey("IdUsuarioSubida")]
        public virtual Usuario? UsuarioSubida { get; set; }

        [Column("FECHA_SUBIDA")]
        public DateTime FechaSubida { get; set; } = DateTime.Now;

        [Column("ES_PUBLICO")]
        public int EsPublico { get; set; } = 1;

        [Column("ELIMINADO")]
        public int Eliminado { get; set; } = 0;
    }
}
