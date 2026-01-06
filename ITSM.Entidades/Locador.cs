using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("ADM_LOCADORES")]
    public class Locador
    {
        [Key]
        [Column("ID_CONTRATO")]
        public int IdContrato { get; set; }

        [Column("ID_LOCADOR")]
        public int? IdLocador { get; set; }

        [ForeignKey("IdLocador")]
        public virtual Usuario? UsuarioLocador { get; set; }

        [Column("NUM_ORDEN_SERVICIO")]
        [StringLength(50)]
        public string? NumOrdenServicio { get; set; }

        [Column("FECHA_INICIO")]
        public DateTime? FechaInicio { get; set; }

        [Column("FECHA_FIN")]
        public DateTime? FechaFin { get; set; }

        [Column("MONTO_TOTAL")]
        public decimal? MontoTotal { get; set; }

        [Column("CANT_ENTREGABLES")]
        public int? CantEntregables { get; set; }
    }
}
