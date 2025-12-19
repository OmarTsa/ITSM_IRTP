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

        [Required]
        [Column("NOMBRE")]
        public string Nombre { get; set; } = string.Empty;

        [Column("HORAS_SLA")]
        public int HorasSLA { get; set; }

        [Column("COLOR")]
        public string? Color { get; set; }
    }
}