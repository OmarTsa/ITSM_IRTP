using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("SYS_NOTIFICACIONES")]
    public class Notificacion
    {
        [Key]
        [Column("ID_NOTIFICACION")]
        public int IdNotificacion { get; set; }

        [Column("ID_USUARIO_DESTINO")]
        public int IdUsuarioDestino { get; set; }

        [ForeignKey("IdUsuarioDestino")]
        public virtual Usuario? UsuarioDestino { get; set; }

        [Column("TIPO_NOTIFICACION")]
        [StringLength(50)]
        public string TipoNotificacion { get; set; } = string.Empty;

        [Column("TITULO")]
        [Required]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [Column("MENSAJE")]
        [StringLength(1000)]
        public string? Mensaje { get; set; }

        [Column("PRIORIDAD")]
        [StringLength(20)]
        public string Prioridad { get; set; } = "NORMAL";

        [Column("LEIDA")]
        public int Leida { get; set; } = 0;

        [Column("FECHA_CREACION")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Column("FECHA_LECTURA")]
        public DateTime? FechaLectura { get; set; }

        [Column("URL_DESTINO")]
        [StringLength(500)]
        public string? UrlDestino { get; set; }

        [Column("ID_REGISTRO_RELACIONADO")]
        public int? IdRegistroRelacionado { get; set; }

        [Column("ICONO")]
        [StringLength(50)]
        public string? Icono { get; set; }

        [Column("COLOR")]
        [StringLength(20)]
        public string? Color { get; set; }
    }
}
