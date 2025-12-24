using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("SEG_ROLES")]
    public class Rol
    {
        [Key]
        [Column("ID_ROL")]
        public int IdRol { get; set; }

        [Column("NOMBRE")]
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty; // Inicializado para robustez

        [Column("DESCRIPCION")]
        [StringLength(200)]
        public string? Descripcion { get; set; }

        [Column("ESTADO")]
        public int Estado { get; set; } = 1;
    }
}