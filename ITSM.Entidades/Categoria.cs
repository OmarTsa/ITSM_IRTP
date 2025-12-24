using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("HD_CATEGORIAS")]
    public class Categoria
    {
        [Key]
        [Column("ID_CATEGORIA")]
        public int IdCategoria { get; set; }

        [Column("NOMBRE")]
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty; // Inicializado

        [Column("TIPO")]
        [StringLength(20)]
        public string? Tipo { get; set; }

        [Column("ACTIVO")]
        public int Activo { get; set; } = 1;
    }
}