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

        [Required]
        [Column("NOMBRE")]
        public string Nombre { get; set; } = string.Empty;

        // Propiedad requerida por el frontend
        [Column("ACTIVO")]
        public int Activo { get; set; } = 1;

        // Opcional: Para diferenciar si es Incidente o Requerimiento
        [Column("TIPO")]
        public string? Tipo { get; set; }
    }
}