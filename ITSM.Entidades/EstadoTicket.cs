using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("ESTADOS_TICKET")]
    public class EstadoTicket
    {
        [Key]
        [Column("ID_ESTADO")]
        public int IdEstado { get; set; }

        [Column("NOMBRE")]
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty; // Inicializado
    }
}