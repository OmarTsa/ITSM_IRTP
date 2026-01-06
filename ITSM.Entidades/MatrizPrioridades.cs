using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("HD_MATRIZ_PRIORIDAD")]
    public class MatrizPrioridad
    {
        [Key]
        [Column("ID_MATRIZ")]
        public int IdMatriz { get; set; }

        [Column("ID_IMPACTO")]
        public int IdImpacto { get; set; }

        [ForeignKey("IdImpacto")]
        public virtual NivelValor? Impacto { get; set; }

        [Column("ID_URGENCIA")]
        public int IdUrgencia { get; set; }

        [ForeignKey("IdUrgencia")]
        public virtual NivelValor? Urgencia { get; set; }

        [Column("ID_PRIORIDAD_RESULTANTE")]
        public int IdPrioridadResultante { get; set; }

        [ForeignKey("IdPrioridadResultante")]
        public virtual Prioridad? PrioridadResultante { get; set; }
    }
}
