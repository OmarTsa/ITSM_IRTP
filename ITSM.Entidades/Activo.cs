using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("ACT_INVENTARIO")]
    public class Activo
    {
        [Key]
        [Column("ID_ACTIVO")]
        public int IdActivo { get; set; }

        // ESTÁNDAR DEFINIDO: Usamos CodInventario
        [Column("CODIGO_PATRIMONIAL")]
        public string CodInventario { get; set; } = string.Empty;

        [Column("MARCA")]
        public string Marca { get; set; } = string.Empty;

        [Column("MODELO")]
        public string Modelo { get; set; } = string.Empty;

        [Column("SERIE")]
        public string Serie { get; set; } = string.Empty;

        [NotMapped]
        public string Nombre => $"{Marca} {Modelo}";

        [Column("ID_USUARIO_ASIGNADO")]
        public int? IdUsuarioAsignado { get; set; }

        [ForeignKey("IdUsuarioAsignado")]
        public Usuario? UsuarioAsignado { get; set; }
    }
}