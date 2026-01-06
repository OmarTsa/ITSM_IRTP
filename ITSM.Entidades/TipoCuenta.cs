using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("CTA_TIPOS_CUENTA")]
    public class TipoCuenta
    {
        [Key]
        [Column("ID_TIPO_CUENTA")]
        public int IdTipoCuenta { get; set; }

        [Column("NOMBRE")]
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;
    }
}
