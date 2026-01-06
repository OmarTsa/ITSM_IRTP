using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("SEG_PERMISOS")]
    public class Permiso
    {
        [Key]
        [Column("ID_PERMISO")]
        public int IdPermiso { get; set; }

        [Column("MODULO")]
        [StringLength(50)]
        public string? Modulo { get; set; }

        [Column("NOMBRE_PERMISO")]
        [Required]
        [StringLength(100)]
        public string NombrePermiso { get; set; } = string.Empty;

        [Column("DESCRIPCION")]
        [StringLength(200)]
        public string? Descripcion { get; set; }

        [Column("CODIGO")]
        [StringLength(50)]
        public string? Codigo { get; set; }

        [Column("ACTIVO")]
        public int Activo { get; set; } = 1;
    }
}
