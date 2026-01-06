using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("ADM_DOCUMENTOS")]
    public class Documento
    {
        [Key]
        [Column("ID_DOCUMENTO")]
        public int IdDocumento { get; set; }

        [Column("ANIO")]
        public int Anio { get; set; }

        [Column("TIPO_DOC")]
        [StringLength(20)]
        public string? TipoDoc { get; set; }

        [Column("NUMERO_CORRELATIVO")]
        public int? NumeroCorrelativo { get; set; }

        [Column("VERSION_DOC")]
        [StringLength(10)]
        public string VersionDoc { get; set; } = "1.0";

        [Column("DESCRIPCION_REQ")]
        [StringLength(500)]
        public string? DescripcionReq { get; set; }

        [Column("ID_AREA_USUARIA")]
        public int? IdAreaUsuaria { get; set; }

        [ForeignKey("IdAreaUsuaria")]
        public virtual Area? AreaUsuaria { get; set; }

        [Column("FECHA_ELABORACION")]
        public DateTime? FechaElaboracion { get; set; }

        [Column("FECHA_PEDIDO_COMPRA")]
        public DateTime? FechaPedidoCompra { get; set; }

        [Column("NUM_PEDIDO_COMPRA")]
        [StringLength(50)]
        public string? NumPedidoCompra { get; set; }

        [Column("NUM_ORDEN")]
        [StringLength(50)]
        public string? NumOrden { get; set; }

        [Column("NUM_PECOSA")]
        [StringLength(50)]
        public string? NumPecosa { get; set; }

        [Column("ESTADO")]
        [StringLength(50)]
        public string? Estado { get; set; }

        [Column("ID_ELABORADO_BY")]
        public int? IdElaboradoBy { get; set; }

        [ForeignKey("IdElaboradoBy")]
        public virtual Usuario? ElaboradoPor { get; set; }

        [Column("FECHA_VALIDACION_TECNICA")]
        public DateTime? FechaValidacionTecnica { get; set; }

        [Column("ID_VALIDADOR_TECNICO")]
        public int? IdValidadorTecnico { get; set; }

        [ForeignKey("IdValidadorTecnico")]
        public virtual Usuario? ValidadorTecnico { get; set; }

        [Column("ESTADO_ACTUAL")]
        [StringLength(50)]
        public string EstadoActual { get; set; } = "ELABORACION";

        [Column("UBICACION_ACTUAL")]
        [StringLength(100)]
        public string? UbicacionActual { get; set; }

        [Column("PRIORIDAD")]
        [StringLength(20)]
        public string Prioridad { get; set; } = "NORMAL";

        [Column("MONTO_ESTIMADO")]
        public decimal? MontoEstimado { get; set; }

        [Column("FECHA_LIMITE")]
        public DateTime? FechaLimite { get; set; }

        [Column("OBSERVACIONES")]
        [StringLength(1000)]
        public string? Observaciones { get; set; }
    }
}
