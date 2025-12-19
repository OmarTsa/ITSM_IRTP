using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("TICKETS")]
    public class Ticket
    {
        [Key]
        [Column("ID_TICKET")]
        public int IdTicket { get; set; }

        // --- Datos Básicos ---
        [Column("TITULO")]
        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [Column("DESCRIPCION")]
        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string? Descripcion { get; set; }

        [Column("TIPO_TICKET")] // 'Incidente' o 'Requerimiento'
        [StringLength(50)]
        public string TipoTicket { get; set; } = "Incidente";

        [Column("FECHA_CREACION")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Column("FECHA_CIERRE")]
        public DateTime? FechaCierre { get; set; }

        // ESTA ES LA PROPIEDAD QUE TE FALTABA EN EL ERROR:
        [NotMapped] // No se guarda en BD Ticket, sirve para buscar el activo
        public string? CodigoPatrimonial { get; set; }

        // --- Relaciones ---

        [Column("ID_SOLICITANTE")]
        public int IdSolicitante { get; set; }
        [ForeignKey("IdSolicitante")]
        public Usuario? Solicitante { get; set; }

        [Column("ID_ACTIVO")]
        public int? IdActivo { get; set; }
        [ForeignKey("IdActivo")]
        public Activo? ActivoRelacionado { get; set; }

        [Column("ID_CATEGORIA")]
        public int IdCategoria { get; set; }
        [ForeignKey("IdCategoria")]
        public Categoria? Categoria { get; set; }

        [Column("ID_PRIORIDAD")]
        public int IdPrioridad { get; set; }
        [ForeignKey("IdPrioridad")]
        public Prioridad? Prioridad { get; set; }

        [Column("ID_ESTADO")]
        public int IdEstado { get; set; }
        [ForeignKey("IdEstado")]
        public EstadoTicket? Estado { get; set; }
    }
}