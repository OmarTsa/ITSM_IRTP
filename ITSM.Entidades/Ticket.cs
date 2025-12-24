using System;
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

        [Column("TITULO")]
        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(200, ErrorMessage = "El título no puede exceder los 200 caracteres")]
        public string Titulo { get; set; }

        [Column("DESCRIPCION")]
        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; }

        [Column("FECHA_CREACION")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Column("FECHA_LIMITE")]
        public DateTime? FechaLimite { get; set; }

        [Column("FECHA_CIERRE")]
        public DateTime? FechaCierre { get; set; }

        [Column("FECHA_ASIGNACION")]
        public DateTime? FechaAsignacion { get; set; }

        // --- RELACIONES Y CLAVES FORÁNEAS ---

        [Column("ID_SOLICITANTE")]
        public int IdSolicitante { get; set; }

        [ForeignKey("IdSolicitante")]
        public virtual Usuario? Solicitante { get; set; }

        [Column("ID_ESPECIALISTA")]
        public int? IdEspecialista { get; set; }

        [ForeignKey("IdEspecialista")]
        public virtual Usuario? Especialista { get; set; }

        [Column("ID_ACTIVO_AFECTADO")]
        public int? IdActivoAfectado { get; set; }

        [ForeignKey("IdActivoAfectado")]
        public virtual Activo? ActivoAfectado { get; set; }

        // --- CLASIFICACIÓN ITIL (Impacto, Urgencia, Prioridad) ---

        [Column("TIPO_TICKET")]
        public string TipoTicket { get; set; } = "Incidente"; // Valores: 'Incidente' o 'Requerimiento'

        [Column("ID_CATEGORIA")]
        public int IdCategoria { get; set; }

        // Asumiendo que existe la entidad Categoria, descomenta si la tienes
        // [ForeignKey("IdCategoria")]
        // public virtual Categoria? Categoria { get; set; }

        [Column("ID_ESTADO")]
        public int IdEstado { get; set; } = 1; // 1 = Abierto por defecto

        // [ForeignKey("IdEstado")]
        // public virtual EstadoTicket? Estado { get; set; }

        [Column("ID_PRIORIDAD")]
        public int IdPrioridad { get; set; } = 3; // 3 = Baja por defecto

        [ForeignKey("IdPrioridad")]
        public virtual Prioridad? Prioridad { get; set; }

        [Column("ID_URGENCIA")]
        public int IdUrgencia { get; set; } = 3; // 1=Alta, 2=Media, 3=Baja

        [Column("ID_IMPACTO")]
        public int IdImpacto { get; set; } = 3;  // 1=Alto, 2=Medio, 3=Bajo

        [Column("ID_ORIGEN")]
        public int? IdOrigen { get; set; }

        // --- CAMPOS DE CIERRE ---

        [Column("CODIGO_CIERRE")]
        [StringLength(50)]
        public string? CodigoCierre { get; set; }

        [Column("NOTAS_CIERRE")]
        [StringLength(500)]
        public string? NotasCierre { get; set; }

        [Column("CALIFICACION")]
        public int? Calificacion { get; set; }
    }
}