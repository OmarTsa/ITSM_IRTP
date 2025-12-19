using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("ACTIVOS")]
    public class Activo
    {
        [Key]
        [Column("ID_ACTIVO")]
        public int IdActivo { get; set; }

        [Required]
        [Column("COD_PATRIMONIAL")]
        [StringLength(50)]
        public string CodPatrimonial { get; set; } = string.Empty;

        [Column("TIPO")]
        [StringLength(50)]
        public string? Tipo { get; set; }

        [Column("MARCA")]
        [StringLength(50)]
        public string? Marca { get; set; }

        [Column("MODELO")]
        [StringLength(50)]
        public string? Modelo { get; set; }

        [Column("FECHA_COMPRA")]
        public DateTime? FechaCompra { get; set; }

        [Column("ID_USUARIO_ASIGNADO")]
        public int? IdUsuarioAsignado { get; set; }

        [ForeignKey("IdUsuarioAsignado")]
        public Usuario? UsuarioAsignado { get; set; }
    }
}