using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("SEG_AREAS")]
    public class Area
    {
        [Key]
        [Column("ID_AREA")]
        public int IdArea { get; set; }

        [Column("NOMBRE")]
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty; // Inicializado

        [Column("SIGLAS")]
        [StringLength(20)]
        public string? Siglas { get; set; }
    }
}