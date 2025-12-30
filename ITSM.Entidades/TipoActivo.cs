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

        // Alias para compatibilidad con servicios
        [NotMapped]
        public int IdTipoActivo
        {
            get => IdTipo;
            set => IdTipo = value;
        }

        [Column("NOMBRE")]
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [Column("DESCRIPCION")]
        [StringLength(200)]
        public string? Descripcion { get; set; }

        [Column("ESTADO")]
        public int Estado { get; set; } = 1; // 1 = Activo, 0 = Inactivo
    }
}
