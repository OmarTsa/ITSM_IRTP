using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    // Asegúrate que el nombre de la tabla coincida con tu BD (ej. TICKETS, GLPI_TICKETS, etc.)
    [Table("GLPI_TICKETS")]
    public class Ticket
    {
        [Key]
        [Column("ID_TICKET")]
        public int IdTicket { get; set; }

        [Column("TITULO")]
        public string Titulo { get; set; } = string.Empty;

        [Column("DESCRIPCION")]
        public string? Descripcion { get; set; }

        [Column("FECHA_CREACION")]
        public DateTime FechaCreacion { get; set; }

        // --- RELACIONES EXISTENTES ---

        [Column("ID_SOLICITANTE")]
        public int IdSolicitante { get; set; }

        [ForeignKey("IdSolicitante")]
        public Usuario? Solicitante { get; set; }

        [Column("ID_CATEGORIA")]
        public int IdCategoria { get; set; }

        [ForeignKey("IdCategoria")]
        public Categoria? Categoria { get; set; }

        [Column("ID_ESTADO")]
        public int IdEstado { get; set; }

        [ForeignKey("IdEstado")]
        public Estado? Estado { get; set; }

        [Column("ID_PRIORIDAD")]
        public int IdPrioridad { get; set; }

        [ForeignKey("IdPrioridad")]
        public Prioridad? Prioridad { get; set; }

        // --- PROPIEDAD QUE FALTABA (SOLUCIÓN DEL ERROR) ---

        [Column("ID_ESPECIALISTA")]
        public int? IdEspecialista { get; set; } // Nullable, porque un ticket nuevo no tiene técnico aún

        [ForeignKey("IdEspecialista")]
        public Usuario? Especialista { get; set; }
    }
}