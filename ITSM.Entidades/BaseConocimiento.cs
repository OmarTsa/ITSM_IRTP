using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("HD_BASE_CONOCIMIENTO")]
    public class BaseConocimiento
    {
        [Key]
        [Column("ID_SOLUCION")]
        public int IdSolucion { get; set; }

        [Column("PROBLEMA")]
        [Required]
        [StringLength(300)]
        public string Problema { get; set; } = string.Empty;

        [Column("SOLUCION")]
        [Required]
        public string Solucion { get; set; } = string.Empty;

        [Column("PLATAFORMA")]
        [StringLength(100)]
        public string? Plataforma { get; set; }

        [Column("FECHA_REGISTRO")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Column("ID_AUTOR")]
        public int IdAutor { get; set; }

        [ForeignKey("IdAutor")]
        public virtual Usuario? Autor { get; set; }

        [Column("VERSION")]
        public int Version { get; set; } = 1;

        [Column("ID_APROBADOR")]
        public int? IdAprobador { get; set; }

        [ForeignKey("IdAprobador")]
        public virtual Usuario? Aprobador { get; set; }

        [Column("FECHA_APROBACION")]
        public DateTime? FechaAprobacion { get; set; }

        [Column("ESTADO")]
        [StringLength(20)]
        public string Estado { get; set; } = "BORRADOR";

        [Column("VISITAS")]
        public int Visitas { get; set; } = 0;

        [Column("UTIL_SI")]
        public int UtilSi { get; set; } = 0;

        [Column("UTIL_NO")]
        public int UtilNo { get; set; } = 0;

        [Column("TAGS")]
        [StringLength(500)]
        public string? Tags { get; set; }

        [Column("CATEGORIA_KB")]
        [StringLength(100)]
        public string? CategoriaKb { get; set; }
    }
}
