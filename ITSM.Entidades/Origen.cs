using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("HD_ORÍGENES")]
    public class Origen
    {
        [Key]
        [Column("ID_ORIGEN")]
        public int IdOrigen { get; set; }

        [Column("NOMBRE")]
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;
    }
}
