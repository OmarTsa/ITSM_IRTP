using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("HD_CORRELATIVOS_TICKET")]
    public class CorrelativoTicket
    {
        [Key]
        [Column("ANIO")]
        public int Anio { get; set; }

        [Column("ULTIMO_CORRELATIVO")]
        public int UltimoCorrelativo { get; set; } = 0;
    }
}
