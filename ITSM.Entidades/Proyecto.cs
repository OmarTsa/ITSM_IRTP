using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("PRY_PROYECTOS")]
    public class Proyecto
    {
        [Key]
        [Column("ID_PROYECTO")]
        public int IdProyecto { get; set; }

        [Column("CODIGO_PROYECTO")]
        [StringLength(20)]
        public string? CodigoProyecto { get; set; }

        [Column("NOMBRE")]
        [Required]
        [StringLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [Column("DESCRIPCION")]
        public string? Descripcion { get; set; }

        [Column("TIPO_PROYECTO")]
        [StringLength(50)]
        public string? TipoProyecto { get; set; }

        [Column("FECHA_INICIO")]
        public DateTime? FechaInicio { get; set; }

        [Column("FECHA_FIN_PREVISTA")]
        public DateTime? FechaFinPrevista { get; set; }

        [Column("FECHA_FIN_REAL")]
        public DateTime? FechaFinReal { get; set; }

        [Column("ESTADO")]
        [StringLength(30)]
        public string Estado { get; set; } = "PLANIFICACION";

        [Column("PRIORIDAD")]
        [StringLength(20)]
        public string Prioridad { get; set; } = "MEDIA";

        [Column("PRESUPUESTO_ASIGNADO")]
        public decimal? PresupuestoAsignado { get; set; }

        [Column("PRESUPUESTO_EJECUTADO")]
        public decimal PresupuestoEjecutado { get; set; } = 0;

        [Column("ID_RESPONSABLE")]
        public int? IdResponsable { get; set; }

        [ForeignKey("IdResponsable")]
        public virtual Usuario? Responsable { get; set; }

        [Column("ID_AREA")]
        public int? IdArea { get; set; }

        [ForeignKey("IdArea")]
        public virtual Area? Area { get; set; }

        [Column("PORCENTAJE_AVANCE")]
        public decimal PorcentajeAvance { get; set; } = 0;

        [Column("ES_ESTRATEGICO")]
        public int EsEstrategico { get; set; } = 0;

        [Column("ALINEADO_PGD")]
        public int AlineadoPgd { get; set; } = 0;

        [Column("FECHA_CREACION")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Column("ELIMINADO")]
        public int Eliminado { get; set; } = 0;
    }
}
