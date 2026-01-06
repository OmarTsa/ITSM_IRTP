using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("SEG_INTENTOS_LOGIN")]
    public class IntentoLogin
    {
        [Key]
        [Column("USERNAME")]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Column("INTENTOS")]
        public int Intentos { get; set; } = 0;

        [Column("ULTIMO_INTENTO")]
        public DateTime? UltimoIntento { get; set; }

        [Column("BLOQUEADO")]
        public int Bloqueado { get; set; } = 0;
    }
}
