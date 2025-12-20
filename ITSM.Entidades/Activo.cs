using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("ACT_INVENTARIO")] // <--- Nombre correcto
    public class Activo
    {
        [Key][Column("ID_ACTIVO")] public int IdActivo { get; set; }
        [Column("CODIGO_PATRIMONIAL")] public string CodPatrimonial { get; set; } = string.Empty;
        // ... resto de propiedades ...
    }
}