using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("SEG_PERMISOS_ROL")]
    public class PermisoRol
    {
        [Key]
        [Column("ID_PERMISO_ROL")]
        public int IdPermisoRol { get; set; }

        [Column("ID_ROL")]
        public int IdRol { get; set; }

        [ForeignKey("IdRol")]
        public virtual Rol? Rol { get; set; }

        [Column("ID_PERMISO")]
        public int IdPermiso { get; set; }

        [ForeignKey("IdPermiso")]
        public virtual Permiso? Permiso { get; set; }

        [Column("PUEDE_CREAR")]
        public int PuedeCrear { get; set; } = 0;

        [Column("PUEDE_LEER")]
        public int PuedeLeer { get; set; } = 0;

        [Column("PUEDE_ACTUALIZAR")]
        public int PuedeActualizar { get; set; } = 0;

        [Column("PUEDE_ELIMINAR")]
        public int PuedeEliminar { get; set; } = 0;

        [Column("PUEDE_EXPORTAR")]
        public int PuedeExportar { get; set; } = 0;

        [Column("FECHA_ASIGNACION")]
        public DateTime FechaAsignacion { get; set; } = DateTime.Now;
    }
}
