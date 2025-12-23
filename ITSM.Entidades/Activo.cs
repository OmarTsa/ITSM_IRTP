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

        [Column("CODIGO_PATRIMONIAL")]
        [Required(ErrorMessage = "El código patrimonial es obligatorio")]
        public string? CodigoPatrimonial { get; set; }

        [Column("ID_TIPO")]
        public int IdTipo { get; set; }

        [Column("MARCA")]
        public string? Marca { get; set; }

        [Column("MODELO")]
        public string? Modelo { get; set; }

        [Column("SERIE")]
        public string? Serie { get; set; }

        [Column("FECHA_COMPRA")]
        public DateTime? FechaCompra { get; set; }

        [Column("CONDICION")]
        public string? Condicion { get; set; }

        [Column("ID_USUARIO_ASIGNADO")]
        public int? IdUsuarioAsignado { get; set; }

        [Column("UBICACION_FISICA")]
        public string? UbicacionFisica { get; set; }

        [Column("ESTADO_OPERATIVO")]
        public string? EstadoOperativo { get; set; }

        [Column("FECHA_REGISTRO")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Column("ELIMINADO")]
        public int Eliminado { get; set; } = 0;

        // Propiedades de Navegación
        [ForeignKey("IdTipo")]
        public virtual TipoActivo? Tipo { get; set; }

        [ForeignKey("IdUsuarioAsignado")]
        public virtual Usuario? UsuarioAsignado { get; set; }
    }
}