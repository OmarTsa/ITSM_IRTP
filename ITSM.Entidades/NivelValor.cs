using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("HD_NIVELES_VALOR")]
    public class NivelValor
    {
        [Key]
        [Column("ID_NIVEL")]
        public int IdNivel { get; set; }

        [Column("NOMBRE")]
        [Required]
        [StringLength(20)]
        public string Nombre { get; set; } = string.Empty;
    }
}
