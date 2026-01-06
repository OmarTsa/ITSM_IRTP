using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("CTA_ACCESOS_USUARIO")]
    public class AccesoUsuario
    {
        [Key]
        [Column("ID_ACCESO")]
        public int IdAcceso { get; set; }

        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuario? Usuario { get; set; }

        [Column("ID_TIPO_CUENTA")]
        public int IdTipoCuenta { get; set; }

        [ForeignKey("IdTipoCuenta")]
        public virtual TipoCuenta? TipoCuenta { get; set; }

        [Column("USUARIO_PLATAFORMA")]
        [StringLength(100)]
        public string? UsuarioPlataforma { get; set; }

        [Column("FECHA_ALTA")]
        public DateTime? FechaAlta { get; set; }

        [Column("DOC_SUSTENTO_ALTA")]
        [StringLength(100)]
        public string? DocSustentoAlta { get; set; }

        [Column("FECHA_BAJA")]
        public DateTime? FechaBaja { get; set; }

        [Column("DOC_SUSTENTO_BAJA")]
        [StringLength(100)]
        public string? DocSustentoBaja { get; set; }

        [Column("ESTADO")]
        [StringLength(20)]
        public string Estado { get; set; } = "VIGENTE";

        [Column("NUMERO_DOCUMENTO_ALTA")]
        [StringLength(50)]
        public string? NumeroDocumentoAlta { get; set; }

        [Column("NUMERO_DOCUMENTO_BAJA")]
        [StringLength(50)]
        public string? NumeroDocumentoBaja { get; set; }

        [Column("CORREO_SUSTENTO_ALTA")]
        [StringLength(200)]
        public string? CorreoSustentoAlta { get; set; }

        [Column("CORREO_SUSTENTO_BAJA")]
        [StringLength(200)]
        public string? CorreoSustentoBaja { get; set; }

        [Column("OBSERVACIONES")]
        [StringLength(500)]
        public string? Observaciones { get; set; }

        [Column("FECHA_CREACION")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Column("FECHA_ULTIMA_MODIFICACION")]
        public DateTime? FechaUltimaModificacion { get; set; }
    }
}
