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
        [StringLength(20)]
        public string CodPatrimonial { get; set; } = string.Empty;

        [Column("MARCA")]
        public string? Marca { get; set; }

        [Column("MODELO")]
        public string? Modelo { get; set; }

        [Column("SERIE")]
        public string? Serie { get; set; }

        [Column("FECHA_COMPRA")]
        public DateTime? FechaCompra { get; set; }

        [Column("ID_USUARIO_ASIGNADO")]
        public int? IdUsuarioAsignado { get; set; }

        [ForeignKey("IdUsuarioAsignado")]
        public Usuario? UsuarioAsignado { get; set; }

        // Propiedades calculadas para la UI de Inventario
        [NotMapped]
        public string Nombre => $"{Marca} {Modelo}".Trim();

        [NotMapped]
        public string CodigoSerie => !string.IsNullOrEmpty(Serie) ? Serie : CodPatrimonial;

        [NotMapped]
        public string TipoActivo { get; set; } = "Equipo Tecnológico";

        [NotMapped]
        public int ActivoSN { get; set; } = 1; // 1 = Operativo
    }
}