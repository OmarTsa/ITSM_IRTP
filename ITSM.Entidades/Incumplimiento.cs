using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("INC_INCUMPLIMIENTOS")]
    public class Incumplimiento
    {
        [Key]
        [Column("ID_INCUMPLIMIENTO")]
        public int IdIncumplimiento { get; set; }

        [Column("ID_TICKET")]
        public int? IdTicket { get; set; }

        [ForeignKey("IdTicket")]
        public virtual Ticket? Ticket { get; set; }

        [Column("ID_PROYECTO")]
        public int? IdProyecto { get; set; }

        [ForeignKey("IdProyecto")]
        public virtual Proyecto? Proyecto { get; set; }

        [Column("TIPO_INCUMPLIMIENTO")]
        [StringLength(50)]
        public string? TipoIncumplimiento { get; set; }

        [Column("DESCRIPCION")]
        [StringLength(1000)]
        public string? Descripcion { get; set; }

        [Column("FECHA_INCUMPLIMIENTO")]
        public DateTime? FechaIncumplimiento { get; set; }

        [Column("GRAVEDAD")]
        [StringLength(20)]
        public string Gravedad { get; set; } = "MEDIA";

        [Column("ID_RESPONSABLE")]
        public int? IdResponsable { get; set; }

        [ForeignKey("IdResponsable")]
        public virtual Usuario? Responsable { get; set; }

        [Column("ID_AREA")]
        public int? IdArea { get; set; }

        [ForeignKey("IdArea")]
        public virtual Area? Area { get; set; }

        [Column("ACCION_CORRECTIVA")]
        [StringLength(1000)]
        public string? AccionCorrectiva { get; set; }

        [Column("FECHA_CORRECCION")]
        public DateTime? FechaCorreccion { get; set; }

        [Column("ESTADO")]
        [StringLength(30)]
        public string Estado { get; set; } = "ABIERTO";

        [Column("ID_REGISTRADO_POR")]
        public int? IdRegistradoPor { get; set; }

        [ForeignKey("IdRegistradoPor")]
        public virtual Usuario? RegistradoPor { get; set; }

        [Column("FECHA_REGISTRO")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Column("MES_REPORTE")]
        public int? MesReporte { get; set; }

        [Column("ANIO_REPORTE")]
        public int? AnioReporte { get; set; }
    }
}
