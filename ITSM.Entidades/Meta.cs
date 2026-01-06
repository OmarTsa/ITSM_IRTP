using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("PRE_METAS")]
    public class Meta
    {
        [Key]
        [Column("ID_META")]
        public int IdMeta { get; set; }

        [Column("ANIO")]
        public int Anio { get; set; }

        [Column("CODIGO_META")]
        [StringLength(20)]
        public string? CodigoMeta { get; set; }

        [Column("ACTIVIDAD_OPERATIVA")]
        [StringLength(50)]
        public string? ActividadOperativa { get; set; }

        [Column("DESCRIPCION")]
        [StringLength(500)]
        public string? Descripcion { get; set; }
    }
}
