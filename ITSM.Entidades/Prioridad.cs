using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("GLPI_PRIORIDAD")] // O el nombre que tenga en tu BD
    public class Prioridad
    {
        [Key]
        [Column("ID_PRIORIDAD")]
        public int IdPrioridad { get; set; }

        [Column("NOMBRE")]
        public string Nombre { get; set; } = string.Empty;

        [Column("DESCRIPCION")]
        public string? Descripcion { get; set; }
    }
}