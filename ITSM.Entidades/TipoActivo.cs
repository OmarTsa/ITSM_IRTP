using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("ACT_TIPOS_ACTIVO")]
    public class TipoActivo
    {
        [Key]
        [Column("ID_TIPO")]
        public int IdTipo { get; set; }

        [Column("NOMBRE")]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty; // Inicializado
    }
}