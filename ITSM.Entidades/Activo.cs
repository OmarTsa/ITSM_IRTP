using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("ACT_INVENTARIO")] // Nombre de tabla según tu Script SQL
    public class Activo
    {
        [Key]
        [Column("ID_ACTIVO")]
        public int IdActivo { get; set; }

        [Column("CODIGO_PATRIMONIAL")]
        public string CodPatrimonial { get; set; } = string.Empty; // Mapeado para que funcione tu Razor

        [Column("MARCA")]
        public string Marca { get; set; } = string.Empty;

        [Column("MODELO")]
        public string Modelo { get; set; } = string.Empty;

        [Column("SERIE")]
        public string Serie { get; set; } = string.Empty;

        // Propiedad calculada "Nombre" para que funcione tu tabla de Razor
        [NotMapped]
        public string Nombre => $"{Marca} {Modelo}";

        [Column("ID_USUARIO_ASIGNADO")]
        public int? IdUsuarioAsignado { get; set; }

        [ForeignKey("IdUsuarioAsignado")]
        public Usuario? UsuarioAsignado { get; set; }
    }
}