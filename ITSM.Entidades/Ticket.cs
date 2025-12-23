using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("HD_TICKETS")]
    public class Ticket
    {
        [Key]
        [Column("ID_TICKET")]
        public int IdTicket { get; set; }

        [Column("FECHA_CREACION")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Column("ID_SOLICITANTE")]
        public int IdSolicitante { get; set; }

        [Column("ID_CATEGORIA")]
        public int IdCategoria { get; set; }

        [Column("TITULO")]
        [Required(ErrorMessage = "El título es obligatorio")]
        public string? Titulo { get; set; }

        [Column("DESCRIPCION")]
        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string? Descripcion { get; set; }

        [Column("ID_ACTIVO_AFECTADO")]
        public int? IdActivoAfectado { get; set; } // <--- Nombre corregido para match con Oracle

        [Column("ID_ESPECIALISTA")]
        public int? IdEspecialista { get; set; }

        [Column("FECHA_ASIGNACION")]
        public DateTime? FechaAsignacion { get; set; }

        [Column("FECHA_CIERRE")]
        public DateTime? FechaCierre { get; set; }

        [Column("CALIFICACION")]
        public int? Calificacion { get; set; }

        [Column("ID_IMPACTO")]
        public int IdImpacto { get; set; } = 3;

        [Column("ID_URGENCIA")]
        public int IdUrgencia { get; set; } = 3;

        [Column("TIPO_TICKET")]
        public string TipoTicket { get; set; } = "Incidente";

        [Column("ORIGEN")]
        public string Origen { get; set; } = "Web";

        [Column("FECHA_LIMITE")]
        public DateTime? FechaLimite { get; set; }

        [Column("CODIGO_CIERRE")]
        public string? CodigoCierre { get; set; }

        [Column("NOTAS_CIERRE")]
        public string? NotasCierre { get; set; }

        [Column("ID_PRIORIDAD")]
        public int IdPrioridad { get; set; } = 3;

        [Column("ID_ESTADO")]
        public int IdEstado { get; set; } = 1;

        // Navegación
        [ForeignKey("IdSolicitante")]
        public virtual Usuario? Solicitante { get; set; }

        [ForeignKey("IdEspecialista")]
        public virtual Usuario? Especialista { get; set; }

        [ForeignKey("IdCategoria")]
        public virtual Categoria? Categoria { get; set; }

        [ForeignKey("IdEstado")]
        public virtual Estado? Estado { get; set; }

        [ForeignKey("IdPrioridad")]
        public virtual Prioridad? Prioridad { get; set; }

        [ForeignKey("IdActivoAfectado")]
        public virtual Activo? Activo { get; set; }
    }
}