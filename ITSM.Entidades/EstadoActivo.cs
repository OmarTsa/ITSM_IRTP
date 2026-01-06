using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("ACT_ESTADOS_ACTIVO")]
    public class EstadoActivo
    {
        [Key]
        [Column("ID_ESTADO")]
        public int IdEstado { get; set; }

        [Required]
        [Column("CODIGO")]
        [StringLength(20)]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        [Column("NOMBRE")]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [Column("DESCRIPCION")]
        [StringLength(200)]
        public string? Descripcion { get; set; }

        [Column("COLOR")]
        [StringLength(20)]
        public string Color { get; set; } = "#666666";

        [Column("ORDEN")]
        public int Orden { get; set; }

        [Column("ACTIVO")]
        public int Activo { get; set; } = 1;

        [Column("PERMITE_ASIGNACION")]
        public int PermiteAsignacion { get; set; } = 1;

        [Column("FECHA_CREACION")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
