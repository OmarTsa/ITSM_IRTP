using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("PRIORIDADES")]
    public class Prioridad
    {
        [Key]
        [Column("ID_PRIORIDAD")]
        public int IdPrioridad { get; set; }

        [Column("NOMBRE")]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty; // Inicializado

        [Column("HORAS_SLA")]
        public int HorasSla { get; set; }

        [Column("COLOR")]
        [StringLength(20)]
        public string? Color { get; set; }
    }
}