using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("PRY_ENTREGABLES")]
    public class Entregable
    {
        [Key]
        [Column("ID_ENTREGABLE")]
        public int IdEntregable { get; set; }

        [Column("ID_PROYECTO")]
        public int IdProyecto { get; set; }

        [ForeignKey("IdProyecto")]
        public virtual Proyecto? Proyecto { get; set; }

        [Column("ID_HITO")]
        public int? IdHito { get; set; }

        [ForeignKey("IdHito")]
        public virtual Hito? Hito { get; set; }

        [Column("NOMBRE_ENTREGABLE")]
        [Required]
        [StringLength(200)]
        public string NombreEntregable { get; set; } = string.Empty;

        [Column("DESCRIPCION")]
        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Column("FECHA_ENTREGA_PREVISTA")]
        public DateTime? FechaEntregaPrevista { get; set; }

        [Column("FECHA_ENTREGA_REAL")]
        public DateTime? FechaEntregaReal { get; set; }

        [Column("RUTA_ARCHIVO")]
        [StringLength(500)]
        public string? RutaArchivo { get; set; }

        [Column("ESTADO")]
        [StringLength(30)]
        public string Estado { get; set; } = "PENDIENTE";

        [Column("APROBADO")]
        public int Aprobado { get; set; } = 0;

        [Column("ID_APROBADOR")]
        public int? IdAprobador { get; set; }

        [ForeignKey("IdAprobador")]
        public virtual Usuario? Aprobador { get; set; }

        [Column("FECHA_APROBACION")]
        public DateTime? FechaAprobacion { get; set; }

        [Column("OBSERVACIONES")]
        [StringLength(1000)]
        public string? Observaciones { get; set; }
    }
}
