using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("ACT_CATALOGO_SOFTWARE")]
    public class CatalogoSoftware
    {
        [Key]
        [Column("ID_SOFTWARE")]
        public int IdSoftware { get; set; }

        [Column("NOMBRE")]
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Column("FABRICANTE")]
        [StringLength(50)]
        public string? Fabricante { get; set; }

        [Column("VERSION")]
        [StringLength(20)]
        public string? Version { get; set; }

        [Column("FECHA_FIN_SOPORTE")]
        public DateTime? FechaFinSoporte { get; set; }

        [Column("ES_LICENCIADO")]
        public int EsLicenciado { get; set; } = 1;

        [Column("COSTO_LICENCIA")]
        public decimal? CostoLicencia { get; set; }

        [Column("LICENCIAS_TOTALES")]
        public int? LicenciasTotales { get; set; }

        [Column("LICENCIAS_USADAS")]
        public int LicenciasUsadas { get; set; } = 0;

        [Column("LICENCIAS_DISPONIBLES")]
        public int? LicenciasDisponibles { get; set; }

        [Column("ALERTA_VENCIMIENTO")]
        public int AlertaVencimiento { get; set; } = 0;

        [Column("DIAS_PARA_VENCIMIENTO")]
        public int? DiasParaVencimiento { get; set; }

        [Column("TIPO_LICENCIA")]
        [StringLength(50)]
        public string? TipoLicencia { get; set; }

        [Column("CLAVE_LICENCIA")]
        [StringLength(200)]
        public string? ClaveLicencia { get; set; }

        [Column("PROVEEDOR_LICENCIA")]
        [StringLength(100)]
        public string? ProveedorLicencia { get; set; }
    }
}
