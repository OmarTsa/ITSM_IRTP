using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("ACT_INSTALACIONES")]
    public class Instalacion
    {
        [Key]
        [Column("ID_INSTALACION")]
        public int IdInstalacion { get; set; }

        [Column("ID_ACTIVO")]
        public int IdActivo { get; set; }

        [ForeignKey("IdActivo")]
        public virtual Activo? Activo { get; set; }

        [Column("ID_SOFTWARE")]
        public int IdSoftware { get; set; }

        [ForeignKey("IdSoftware")]
        public virtual CatalogoSoftware? Software { get; set; }

        [Column("FECHA_INSTALACION")]
        public DateTime FechaInstalacion { get; set; } = DateTime.Now;

        [Column("TIENE_LICENCIA")]
        public int TieneLicencia { get; set; } = 1;

        [Column("OBSERVACIONES")]
        [StringLength(200)]
        public string? Observaciones { get; set; }
    }
}
