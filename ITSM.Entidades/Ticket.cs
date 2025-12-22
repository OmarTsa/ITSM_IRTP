using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
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

        // --- RELACIONES ---

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

        [Column("ID_ESPECIALISTA")]
        public int? IdEspecialista { get; set; }

        [ForeignKey("IdEspecialista")]
        public Usuario? Especialista { get; set; }

        // --- ESTO SOLUCIONA EL ERROR CS1061 EN NUEVO TICKET ---
        [Column("ID_ACTIVO")]
        public int? IdActivo { get; set; }

        [ForeignKey("IdActivo")]
        public Activo? Activo { get; set; }
    }
}