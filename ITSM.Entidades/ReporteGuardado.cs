using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("RPT_REPORTES_GUARDADOS")]
    public class ReporteGuardado
    {
        [Key]
        [Column("ID_REPORTE")]
        public int IdReporte { get; set; }

        [Column("NOMBRE_REPORTE")]
        [Required]
        [StringLength(200)]
        public string NombreReporte { get; set; } = string.Empty;

        [Column("DESCRIPCION")]
        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Column("TIPO_REPORTE")]
        [StringLength(50)]
        public string? TipoReporte { get; set; }

        [Column("MODULO")]
        [StringLength(50)]
        public string? Modulo { get; set; }

        [Column("PARAMETROS_JSON")]
        public string? ParametrosJson { get; set; }

        [Column("CONSULTA_SQL")]
        public string? ConsultaSql { get; set; }

        [Column("ID_USUARIO_CREADOR")]
        public int? IdUsuarioCreador { get; set; }

        [ForeignKey("IdUsuarioCreador")]
        public virtual Usuario? UsuarioCreador { get; set; }

        [Column("ES_PUBLICO")]
        public int EsPublico { get; set; } = 0;

        [Column("ES_PROGRAMADO")]
        public int EsProgramado { get; set; } = 0;

        [Column("FRECUENCIA_PROGRAMACION")]
        [StringLength(30)]
        public string? FrecuenciaProgramacion { get; set; }

        [Column("PROXIMA_EJECUCION")]
        public DateTime? ProximaEjecucion { get; set; }

        [Column("DESTINATARIOS_EMAIL")]
        [StringLength(1000)]
        public string? DestinatariosEmail { get; set; }

        [Column("FORMATO_EXPORTACION")]
        [StringLength(20)]
        public string FormatoExportacion { get; set; } = "PDF";

        [Column("FECHA_CREACION")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Column("FECHA_ULTIMA_EJECUCION")]
        public DateTime? FechaUltimaEjecucion { get; set; }

        [Column("ACTIVO")]
        public int Activo { get; set; } = 1;
    }
}
