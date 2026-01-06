using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("RPT_HISTORIAL_EJECUCIONES")]
    public class HistorialEjecucion
    {
        [Key]
        [Column("ID_EJECUCION")]
        public int IdEjecucion { get; set; }

        [Column("ID_REPORTE")]
        public int? IdReporte { get; set; }

        [ForeignKey("IdReporte")]
        public virtual ReporteGuardado? Reporte { get; set; }

        [Column("FECHA_EJECUCION")]
        public DateTime FechaEjecucion { get; set; } = DateTime.Now;

        [Column("TIEMPO_EJECUCION_MS")]
        public int? TiempoEjecucionMs { get; set; }

        [Column("FILAS_RETORNADAS")]
        public int? FilasRetornadas { get; set; }

        [Column("RUTA_ARCHIVO")]
        [StringLength(500)]
        public string? RutaArchivo { get; set; }

        [Column("ESTADO")]
        [StringLength(20)]
        public string? Estado { get; set; }

        [Column("MENSAJE_ERROR")]
        [StringLength(1000)]
        public string? MensajeError { get; set; }

        [Column("ID_USUARIO_EJECUTOR")]
        public int? IdUsuarioEjecutor { get; set; }

        [ForeignKey("IdUsuarioEjecutor")]
        public virtual Usuario? UsuarioEjecutor { get; set; }
    }
}
