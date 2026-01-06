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

        // ⭐ NUEVO: Código correlativo único generado por BD
        [Column("CODIGO_TICKET")]
        [StringLength(20)]
        public string? CodigoTicket { get; set; }

        [Column("TITULO")]
        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [Column("DESCRIPCION")]
        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; } = string.Empty;

        [Column("FECHA_CREACION")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Column("FECHA_LIMITE")]
        public DateTime? FechaLimite { get; set; }

        [Column("FECHA_CIERRE")]
        public DateTime? FechaCierre { get; set; }

        [Column("FECHA_ASIGNACION")]
        public DateTime? FechaAsignacion { get; set; }

        // --- RELACIONES CON USUARIOS ---
        [Column("ID_SOLICITANTE")]
        public int IdSolicitante { get; set; }

        [ForeignKey("IdSolicitante")]
        public virtual Usuario? Solicitante { get; set; }

        [Column("ID_ESPECIALISTA")]
        public int? IdEspecialista { get; set; }

        [ForeignKey("IdEspecialista")]
        public virtual Usuario? Especialista { get; set; }

        // --- RELACIÓN CON ACTIVOS ---
        [Column("ID_ACTIVO_AFECTADO")]
        public int? IdActivoAfectado { get; set; }

        [ForeignKey("IdActivoAfectado")]
        public virtual Activo? ActivoAfectado { get; set; }

        // ⭐ NUEVO: Trazabilidad de área del solicitante
        [Column("ID_AREA_SOLICITANTE")]
        public int? IdAreaSolicitante { get; set; }

        [ForeignKey("IdAreaSolicitante")]
        public virtual Area? AreaSolicitante { get; set; }

        // --- CLASIFICACIÓN ---
        [Column("TIPO_TICKET")]
        public string TipoTicket { get; set; } = "Incidente";

        [Column("ID_CATEGORIA")]
        public int IdCategoria { get; set; }

        [ForeignKey("IdCategoria")]
        public virtual Categoria? Categoria { get; set; }

        [Column("ID_ESTADO")]
        public int IdEstado { get; set; } = 1;

        [ForeignKey("IdEstado")]
        public virtual EstadoTicket? Estado { get; set; }

        [Column("ID_PRIORIDAD")]
        public int IdPrioridad { get; set; } = 3;

        [ForeignKey("IdPrioridad")]
        public virtual Prioridad? Prioridad { get; set; }

        [Column("ID_URGENCIA")]
        public int IdUrgencia { get; set; } = 3;

        [Column("ID_IMPACTO")]
        public int IdImpacto { get; set; } = 3;

        [Column("ID_ORIGEN")]
        public int? IdOrigen { get; set; }

        [Column("CODIGO_CIERRE")]
        [StringLength(50)]
        public string? CodigoCierre { get; set; }

        [Column("NOTAS_CIERRE")]
        [StringLength(500)]
        public string? NotasCierre { get; set; }

        [Column("CALIFICACION")]
        public int? Calificacion { get; set; }

        // ⭐ PROPIEDADES CALCULADAS PARA REPORTES (no mapeadas a BD)
        [NotMapped]
        public string CodigoPatrimonialBien => ActivoAfectado?.CodigoPatrimonial ?? "N/A";

        [NotMapped]
        public string NombreCompletoSolicitante => Solicitante != null
            ? $"{Solicitante.Nombres} {Solicitante.Apellidos}"
            : "Sin asignar";

        [NotMapped]
        public string AreaSolicitanteNombre => AreaSolicitante?.Nombre ??
                                                Solicitante?.Area?.Nombre ?? "N/A";
    }
}
