using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("SYS_CONFIGURACION")]
    public class ConfiguracionSistema
    {
        [Key]
        [Column("ID_CONFIG")]
        public int IdConfig { get; set; }

        [Column("CLAVE")]
        [Required]
        [StringLength(100)]
        public string Clave { get; set; } = string.Empty;

        [Column("VALOR")]
        [StringLength(500)]
        public string? Valor { get; set; }

        [Column("DESCRIPCION")]
        [StringLength(200)]
        public string? Descripcion { get; set; }

        [Column("TIPO_DATO")]
        [StringLength(20)]
        public string TipoDato { get; set; } = "STRING";

        [Column("MODULO")]
        [StringLength(50)]
        public string? Modulo { get; set; }

        [Column("FECHA_MODIFICACION")]
        public DateTime FechaModificacion { get; set; } = DateTime.Now;

        [Column("ID_USUARIO_MODIFICACION")]
        public int? IdUsuarioModificacion { get; set; }

        [ForeignKey("IdUsuarioModificacion")]
        public virtual Usuario? UsuarioModificacion { get; set; }
    }
}
