using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("AUDITORIA_LOGS")]
    public class AuditoriaLog
    {
        [Key]
        [Column("ID_LOG")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdLog { get; set; }

        [Column("ID_USUARIO")]
        public int? IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuario? Usuario { get; set; }

        [Column("ACCION")]
        [StringLength(100)]
        public string? Accion { get; set; }

        [Column("TABLA_AFECTADA")]
        [StringLength(50)]
        public string? TablaAfectada { get; set; }

        [Column("FECHA")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Column("DETALLE")]
        [StringLength(500)]
        public string? Detalle { get; set; }

        [Column("IP_DIRECCION")]
        [StringLength(45)]
        public string? IpDireccion { get; set; }

        [Column("NIVEL_SEVERIDAD")]
        [StringLength(10)]
        public string NivelSeveridad { get; set; } = "INFO";

        [Column("MODULO")]
        [StringLength(50)]
        public string? Modulo { get; set; }

        [Column("MODULO_AFECTADO")]
        [StringLength(50)]
        public string? ModuloAfectado { get; set; }

        [Column("IP_CLIENTE")]
        [StringLength(45)]
        public string? IpCliente { get; set; }

        [Column("METODO_HTTP")]
        [StringLength(10)]
        public string? MetodoHttp { get; set; }

        [Column("NAVEGADOR")]
        [StringLength(200)]
        public string? Navegador { get; set; }

        [Column("SISTEMA_OPERATIVO")]
        [StringLength(100)]
        public string? SistemaOperativo { get; set; }

        [Column("ID_REGISTRO_AFECTADO")]
        public int? IdRegistroAfectado { get; set; }

        [Column("VALOR_ANTERIOR")]
        public string? ValorAnterior { get; set; }

        [Column("VALOR_NUEVO")]
        public string? ValorNuevo { get; set; }
    }
}
