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

        [Required]
        [Column("CODIGO_PATRIMONIAL")]
        [StringLength(50)]
        public string CodPatrimonial { get; set; } = string.Empty;

        [Column("MARCA")]
        [StringLength(100)]
        public string? Marca { get; set; }

        [Column("MODELO")]
        [StringLength(100)]
        public string? Modelo { get; set; }

        [Column("SERIE")]
        [StringLength(100)]
        public string? Serie { get; set; }

        [Column("TIPO_ACTIVO")]
        [StringLength(50)]
        public string? TipoActivo { get; set; }

        [Column("ESTADO_OPERATIVO")]
        public int ActivoSN { get; set; } // 1: Operativo, 0: No Operativo

        [Column("ID_USUARIO_ASIGNADO")]
        public int? IdUsuarioAsignado { get; set; }

        [ForeignKey("IdUsuarioAsignado")]
        public Usuario? UsuarioAsignado { get; set; }

        // Propiedad calculada para mostrar en tablas y combos
        [NotMapped]
        public string Nombre => $"{Marca} {Modelo} (S/N: {Serie})".Trim();

        [NotMapped]
        public string CodigoSerie => !string.IsNullOrEmpty(Serie) ? Serie : CodPatrimonial;
    }
}