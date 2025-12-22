using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("SEG_ESTADOS")] // Verifica si tu tabla se llama así o "GLPI_ESTADOS" en tu BD
    public class Estado
    {
        [Key]
        [Column("ID_ESTADO")]
        public int IdEstado { get; set; }

        [Column("NOMBRE")]
        public string Nombre { get; set; } = string.Empty;

        [Column("ACTIVO")]
        public int? Activo { get; set; }
    }
}