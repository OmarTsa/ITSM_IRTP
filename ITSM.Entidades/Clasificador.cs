using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("PRE_CLASIFICADORES")]
    public class Clasificador
    {
        [Key]
        [Column("ID_CLASIFICADOR")]
        public int IdClasificador { get; set; }

        [Column("CODIGO")]
        [StringLength(50)]
        public string? Codigo { get; set; }

        [Column("DESCRIPCION")]
        [StringLength(200)]
        public string? Descripcion { get; set; }
    }
}
