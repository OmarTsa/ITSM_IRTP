using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("SEG_INTENTOS_ACCESO")]
    public class IntentoAcceso
    {
        [Key]
        [Column("ID_INTENTO")]
        public int IdIntento { get; set; }

        [Column("USERNAME")]
        [StringLength(50)]
        public string? Username { get; set; }

        [Column("IP_ORIGEN")]
        [StringLength(45)]
        public string? IpOrigen { get; set; }

        [Column("FECHA_INTENTO")]
        public DateTime FechaIntento { get; set; } = DateTime.Now;

        [Column("EXITOSO")]
        public int Exitoso { get; set; }
    }
}
