using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("PRE_EJECUCION")]
    public class Ejecucion
    {
        [Key]
        [Column("ID_EJECUCION")]
        public int IdEjecucion { get; set; }

        [Column("ID_META")]
        public int? IdMeta { get; set; }

        [ForeignKey("IdMeta")]
        public virtual Meta? Meta { get; set; }

        [Column("ID_CLASIFICADOR")]
        public int? IdClasificador { get; set; }

        [ForeignKey("IdClasificador")]
        public virtual Clasificador? Clasificador { get; set; }

        [Column("MONTO_PIA")]
        public decimal MontoPia { get; set; } = 0;

        [Column("MONTO_PIM")]
        public decimal MontoPim { get; set; } = 0;

        [Column("MONTO_CERTIFICADO")]
        public decimal MontoCertificado { get; set; } = 0;

        [Column("MONTO_DEVENGADO")]
        public decimal MontoDevengado { get; set; } = 0;

        [Column("FECHA_ACTUALIZACION")]
        public DateTime FechaActualizacion { get; set; } = DateTime.Now;
    }
}
