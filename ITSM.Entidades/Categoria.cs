using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("CATEGORIAS")]
    public class Categoria
    {
        [Key]
        [Column("ID_CATEGORIA")]
        public int IdCategoria { get; set; }

        [Required]
        [Column("NOMBRE")]
        public string Nombre { get; set; } = string.Empty;

        [Column("ACTIVO")]
        public int Activo { get; set; } = 1;
    }
}