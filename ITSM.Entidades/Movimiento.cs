using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("ACT_MOVIMIENTOS")]
    public class Movimiento
    {
        [Key]
        [Column("ID_MOVIMIENTO")]
        public int IdMovimiento { get; set; }

        [Column("TIPO_MOVIMIENTO")]
        [Required]
        [StringLength(50)]
        public string TipoMovimiento { get; set; } = string.Empty;

        [Column("ID_ACTIVO")]
        public int IdActivo { get; set; }

        [ForeignKey("IdActivo")]
        public virtual Activo? Activo { get; set; }

        [Column("ID_USUARIO_ORIGEN")]
        public int? IdUsuarioOrigen { get; set; }

        [ForeignKey("IdUsuarioOrigen")]
        public virtual Usuario? UsuarioOrigen { get; set; }

        [Column("ID_USUARIO_DESTINO")]
        public int? IdUsuarioDestino { get; set; }

        [ForeignKey("IdUsuarioDestino")]
        public virtual Usuario? UsuarioDestino { get; set; }

        [Column("FECHA_MOVIMIENTO")]
        public DateTime FechaMovimiento { get; set; } = DateTime.Now;

        [Column("MOTIVO")]
        [StringLength(500)]
        public string? Motivo { get; set; }

        [Column("RUTA_PDF_FIRMADO")]
        [StringLength(500)]
        public string? RutaPdfFirmado { get; set; }

        [Column("ID_TECNICO_RESPONSABLE")]
        public int? IdTecnicoResponsable { get; set; }

        [ForeignKey("IdTecnicoResponsable")]
        public virtual Usuario? TecnicoResponsable { get; set; }

        [Column("HASH_PDF")]
        [StringLength(256)]
        public string? HashPdf { get; set; }

        [Column("FIRMA_DIGITAL")]
        public byte[]? FirmaDigital { get; set; }

        [Column("ESTADO_FIRMA")]
        [StringLength(20)]
        public string EstadoFirma { get; set; } = "PENDIENTE";

        [Column("FECHA_FIRMA")]
        public DateTime? FechaFirma { get; set; }

        [Column("ID_FIRMANTE")]
        public int? IdFirmante { get; set; }

        [ForeignKey("IdFirmante")]
        public virtual Usuario? Firmante { get; set; }

        [Column("CERTIFICADO_INFO")]
        [StringLength(500)]
        public string? CertificadoInfo { get; set; }

        [Column("OBSERVACIONES_FIRMA")]
        [StringLength(500)]
        public string? ObservacionesFirma { get; set; }

        [Column("NUMERO_PAPELETA")]
        [StringLength(50)]
        public string? NumeroPapeleta { get; set; }

        [Column("TIPO_DOCUMENTO")]
        [StringLength(30)]
        public string TipoDocumento { get; set; } = "DESPLAZAMIENTO";
    }
}
