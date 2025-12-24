using System;
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
        [StringLength(20)]
        public string CodigoPatrimonial { get; set; }

        [Column("ID_TIPO")]
        public int IdTipo { get; set; }

        [ForeignKey("IdTipo")]
        public virtual TipoActivo? TipoActivo { get; set; }

        [Column("MARCA")]
        [StringLength(50)]
        public string? Marca { get; set; }

        [Column("MODELO")]
        [StringLength(50)]
        public string? Modelo { get; set; }

        [Column("SERIE")]
        [StringLength(50)]
        public string? Serie { get; set; }

        [Column("FECHA_COMPRA")]
        public DateTime? FechaCompra { get; set; }

        [Column("CONDICION")]
        [StringLength(20)]
        public string? Condicion { get; set; } // Ej: Bueno, Regular, Malo

        [Column("ID_USUARIO_ASIGNADO")]
        public int? IdUsuarioAsignado { get; set; }

        [ForeignKey("IdUsuarioAsignado")]
        public virtual Usuario? UsuarioAsignado { get; set; }

        [Column("UBICACION_FISICA")]
        [StringLength(100)]
        public string? UbicacionFisica { get; set; }

        [Column("ESTADO_OPERATIVO")]
        [StringLength(20)]
        public string? EstadoOperativo { get; set; } // Ej: Operativo, En Taller

        [Column("FECHA_REGISTRO")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Column("ELIMINADO")]
        public int Eliminado { get; set; } = 0; // Borrado lógico (0=No, 1=Si)
    }
}