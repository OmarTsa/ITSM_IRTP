using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    // Coincide con CREATE TABLE "SEG_USUARIOS"
    [Table("SEG_USUARIOS")]
    public class Usuario
    {
        [Key]
        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }

        [Column("DNI")]
        public string? Dni { get; set; } // Puede ser nulo según script si no es obligatorio

        // --- CORRECCIÓN: Mapeamos NOMBRES y APELLIDOS por separado ---
        [Column("NOMBRES")]
        public string Nombres { get; set; } = string.Empty;

        [Column("APELLIDOS")]
        public string Apellidos { get; set; } = string.Empty;

        // Propiedad calculada: No se guarda en BD, pero la usas en la Web
        [NotMapped]
        public string NombreCompleto => $"{Nombres} {Apellidos}";
        // ----------------------------------------------

        [Column("CORREO")]
        public string Correo { get; set; } = string.Empty;

        // En tu script la columna es USERNAME
        [Column("USERNAME")]
        public string NombreUsuario { get; set; } = string.Empty;

        // En tu script la columna es PASSWORD_HASH
        [Column("PASSWORD_HASH")]
        public string Clave { get; set; } = string.Empty;

        [Column("ID_ROL")]
        public int IdRol { get; set; }

        [Column("ID_AREA")]
        public int? IdArea { get; set; } // Puede ser nulo

        [Column("CARGO")]
        public string? Cargo { get; set; }

        [Column("ESTADO")]
        public int Estado { get; set; } = 1;

        // Relaciones (Foreign Keys)
        [ForeignKey("IdRol")]
        public Rol? Rol { get; set; }

        // Propiedades auxiliares para que tu código actual no se rompa
        [NotMapped] public string Username { get => NombreUsuario; set => NombreUsuario = value; }
        [NotMapped] public string PasswordHash { get => Clave; set => Clave = value; }
    }
}