using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades;

[Table("INV_ACTIVOS")]
public class Activo
{
    [Key]
    [Column("ID_ACTIVO")]
    public int IdActivo { get; set; }

    [Column("NOMBRE")]
    public string Nombre { get; set; } = string.Empty;

    [Column("CODIGO_INVENTARIO")]
    public string CodigoInventario { get; set; } = string.Empty;

    // Campos faltantes que pide Inventario.razor
    [Column("COD_PATRIMONIAL")]
    public string CodPatrimonial { get; set; } = string.Empty;

    [Column("SERIE")]
    public string Serie { get; set; } = string.Empty;

    [Column("ID_USUARIO_ASIGNADO")]
    public int? IdUsuarioAsignado { get; set; }

    [Column("ACTIVO_SN")]
    public string ActivoSN { get; set; } = "S"; // Para el checkbox S/N

    [ForeignKey("IdUsuarioAsignado")]
    public Usuario? UsuarioAsignado { get; set; }
}