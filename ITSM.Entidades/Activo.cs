using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    // CORREGIDO: Nombre de tabla según Script Oracle
    [Table("ACT_INVENTARIO")]
    public class Activo
    {
        [Key]
        [Column("ID_ACTIVO")]
        public int IdActivo { get; set; }

        // El script no tenía NOMBRE, tenía MARCA/MODELO. 
        // Si agregaste NOMBRE después, déjalo. Si no, mapealo a MARCA o concatena.
        [Column("MARCA")]
        public string Nombre { get; set; } = string.Empty;

        [Column("MODELO")]
        public string Modelo { get; set; } = string.Empty;

        // CORREGIDO: Nombre de columna según Script
        [Column("CODIGO_PATRIMONIAL")]
        public string CodPatrimonial { get; set; } = string.Empty;

        [Column("SERIE")]
        public string Serie { get; set; } = string.Empty;

        [Column("ID_USUARIO_ASIGNADO")]
        public int? IdUsuarioAsignado { get; set; }

        // Usamos NotMapped si esta columna no existe en la BD aún
        [NotMapped]
        public string ActivoSN { get; set; } = "S";

        [ForeignKey("IdUsuarioAsignado")]
        public Usuario? UsuarioAsignado { get; set; }
    }
}