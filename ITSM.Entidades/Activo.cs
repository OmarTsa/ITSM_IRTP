using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("ACT_INVENTARIO")]
    public class Activo
    {
        [Key]
        [Column("ID_ACTIVO")]
        public int IdActivo { get; set; }

        // ===================================================================
        // INFORMACIÓN BÁSICA
        // ===================================================================

        [Required]
        [Column("NOMBRE")]
        [StringLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [Column("DESCRIPCION")]
        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Column("CODIGO_PATRIMONIAL")]
        [StringLength(20)]
        public string? CodigoPatrimonial { get; set; }

        [Column("ID_TIPO")]
        public int? IdTipo { get; set; }

        [Column("MARCA")]
        [StringLength(50)]
        public string? Marca { get; set; }

        [Column("MODELO")]
        [StringLength(50)]
        public string? Modelo { get; set; }

        [Column("NUMERO_SERIE")]
        [StringLength(100)]
        public string? NumeroSerie { get; set; }

        // ===================================================================
        // ESPECIFICACIONES TÉCNICAS
        // ===================================================================

        [Column("PROCESADOR")]
        [StringLength(100)]
        public string? Procesador { get; set; }

        [Column("RAM")]
        [StringLength(50)]
        public string? Ram { get; set; }

        [Column("RAM_GB")]
        public int? RamGb { get; set; }

        [Column("ALMACENAMIENTO")]
        [StringLength(50)]
        public string? Almacenamiento { get; set; }

        [Column("DISCO_GB")]
        public int? DiscoGb { get; set; }

        [Column("TIPO_DISCO")]
        [StringLength(20)]
        public string? TipoDisco { get; set; }

        [Column("SISTEMA_OPERATIVO")]
        [StringLength(100)]
        public string? SistemaOperativo { get; set; }

        [Column("VERSION_SO")]
        [StringLength(50)]
        public string? VersionSo { get; set; }

        // ===================================================================
        // CONFIGURACIÓN DE RED
        // ===================================================================

        [Column("HOSTNAME")]
        [StringLength(100)]
        public string? Hostname { get; set; }

        [Column("IP_ASIGNADA")]
        [StringLength(15)]
        public string? IpAsignada { get; set; }

        [Column("MAC_ADDRESS")]
        [StringLength(17)]
        public string? MacAddress { get; set; }

        // ===================================================================
        // ESTADO Y ASIGNACIÓN
        // ===================================================================

        [Column("ID_ESTADO")]
        public int? IdEstado { get; set; }

        [Column("ID_USUARIO_ASIGNADO")]
        public int? IdUsuarioAsignado { get; set; }

        [Column("FECHA_ASIGNACION")]
        public DateTime? FechaAsignacion { get; set; }

        [Column("UBICACION_FISICA")]
        [StringLength(100)]
        public string? UbicacionFisica { get; set; }

        // ===================================================================
        // INFORMACIÓN FINANCIERA
        // ===================================================================

        [Column("FECHA_ADQUISICION")]
        public DateTime? FechaAdquisicion { get; set; }

        [Column("PRECIO_ADQUISICION")]
        public decimal? PrecioAdquisicion { get; set; }

        [Column("PROVEEDOR")]
        [StringLength(100)]
        public string? Proveedor { get; set; }

        [Column("NUMERO_PEDIDO")]
        [StringLength(50)]
        public string? NumeroPedido { get; set; }

        [Column("FECHA_GARANTIA_HASTA")]
        public DateTime? FechaGarantiaHasta { get; set; }

        // ===================================================================
        // CICLO DE VIDA
        // ===================================================================

        [Column("VIDA_UTIL_ANIOS")]
        public int? VidaUtilAnios { get; set; } = 6;

        [Column("ANIOS_ANTIGUEDAD")]
        public int? AniosAntiguedad { get; set; }

        [Column("ALERTA_BRECHA_TECNOLOGICA")]
        public int AlertaBrechaTecnologica { get; set; } = 0;

        [Column("FECHA_BAJA")]
        public DateTime? FechaBaja { get; set; }

        [Column("MOTIVO_BAJA")]
        [StringLength(500)]
        public string? MotivoBaja { get; set; }

        // ===================================================================
        // AUDITORÍA
        // ===================================================================

        [Column("OBSERVACIONES")]
        [StringLength(1000)]
        public string? Observaciones { get; set; }

        [Column("FECHA_REGISTRO")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Column("FECHA_MODIFICACION")]
        public DateTime? FechaModificacion { get; set; }

        [Column("ID_USUARIO_REGISTRO")]
        public int? IdUsuarioRegistro { get; set; }

        [Column("ID_USUARIO_MODIFICACION")]
        public int? IdUsuarioModificacion { get; set; }

        [Column("ELIMINADO")]
        public int Eliminado { get; set; } = 0;

        // ===================================================================
        // CAMPOS LEGACY (Mantener compatibilidad)
        // ===================================================================

        [Column("SERIE")]
        [StringLength(50)]
        public string? Serie { get; set; }

        [Column("FECHA_COMPRA")]
        public DateTime? FechaCompra { get; set; }

        [Column("CONDICION")]
        [StringLength(20)]
        public string? Condicion { get; set; }

        [Column("ESTADO_OPERATIVO")]
        [StringLength(20)]
        public string? EstadoOperativo { get; set; }

        [Column("COSTO_COMPRA")]
        public decimal? CostoCompra { get; set; }

        // ===================================================================
        // PROPIEDADES DE NAVEGACIÓN
        // ===================================================================

        [ForeignKey("IdTipo")]
        public virtual TipoActivo? TipoActivo { get; set; }

        [ForeignKey("IdEstado")]
        public virtual EstadoActivo? Estado { get; set; }

        [ForeignKey("IdUsuarioAsignado")]
        public virtual Usuario? UsuarioAsignado { get; set; }

        // ===================================================================
        // PROPIEDADES CALCULADAS
        // ===================================================================

        [NotMapped]
        public string EstadoGarantia => 
            FechaGarantiaHasta.HasValue && FechaGarantiaHasta.Value >= DateTime.Now 
                ? "VIGENTE" 
                : "VENCIDA";
    }
}
