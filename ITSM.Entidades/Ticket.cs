using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("HD_TICKETS")] // <--- CRÍTICO: Tabla correcta
    public class Ticket
    {
        [Key]
        [Column("ID_TICKET")]
        public int IdTicket { get; set; }

        // --- DATOS PRINCIPALES ---
        [Column("TITULO")]
        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [Column("DESCRIPCION")]
        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; } = string.Empty;

        [Column("TIPO_TICKET")]
        public string TipoTicket { get; set; } = "Incidente";

        [Column("ORIGEN")]
        public string Origen { get; set; } = "Web";

        // --- MATRIZ ITIL (Impacto + Urgencia = Prioridad) ---
        [Column("ID_IMPACTO")]
        public int IdImpacto { get; set; } = 3; // 3=Bajo por defecto

        [Column("ID_URGENCIA")]
        public int IdUrgencia { get; set; } = 3; // 3=Bajo por defecto

        [Column("ID_PRIORIDAD")]
        public int IdPrioridad { get; set; }

        [ForeignKey("IdPrioridad")]
        public Prioridad? Prioridad { get; set; }

        // --- SLA Y TIEMPOS ---
        [Column("FECHA_CREACION")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Column("FECHA_LIMITE")] // SLA Target
        public DateTime? FechaLimite { get; set; }

        [Column("FECHA_CIERRE")]
        public DateTime? FechaCierre { get; set; }

        // --- CIERRE ---
        [Column("CODIGO_CIERRE")]
        public string? CodigoCierre { get; set; }

        [Column("NOTAS_CIERRE")]
        public string? NotasCierre { get; set; }

        // --- RELACIONES ---
        [Column("ID_SOLICITANTE")]
        public int IdSolicitante { get; set; }
        [ForeignKey("IdSolicitante")]
        public Usuario? Solicitante { get; set; }

        // En tu script SQL profesional usamos ID_ACTIVO_AFECTADO
        [Column("ID_ACTIVO_AFECTADO")]
        public int? IdActivo { get; set; }
        [ForeignKey("IdActivo")]
        public Activo? ActivoRelacionado { get; set; }

        [Column("ID_CATEGORIA")]
        public int IdCategoria { get; set; }
        [ForeignKey("IdCategoria")]
        public Categoria? Categoria { get; set; }

        [Column("ID_ESTADO")]
        public int IdEstado { get; set; }
        [ForeignKey("IdEstado")]
        public EstadoTicket? Estado { get; set; }

        // --- AUXILIARES (No BD) ---
        [NotMapped]
        public string? CodigoPatrimonial { get; set; }
    }
}