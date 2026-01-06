using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("PRY_HITOS")]
    public class Hito
    {
        [Key]
        [Column("ID_HITO")]
        public int IdHito { get; set; }

        [Column("ID_PROYECTO")]
        public int IdProyecto { get; set; }

        [ForeignKey("IdProyecto")]
        public virtual Proyecto? Proyecto { get; set; }

        [Column("NOMBRE_HITO")]
        [Required]
        [StringLength(200)]
        public string NombreHito { get; set; } = string.Empty;

        [Column("DESCRIPCION")]
        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Column("FECHA_PREVISTA")]
        public DateTime? FechaPrevista { get; set; }

        [Column("FECHA_COMPLETADO")]
        public DateTime? FechaCompletado { get; set; }

        [Column("ESTADO")]
        [StringLength(30)]
        public string Estado { get; set; } = "PENDIENTE";

        [Column("ORDEN")]
        public int? Orden { get; set; }

        [Column("ID_RESPONSABLE")]
        public int? IdResponsable { get; set; }

        [ForeignKey("IdResponsable")]
        public virtual Usuario? Responsable { get; set; }
    }
}
