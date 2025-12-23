using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    [Table("SEG_USUARIOS")]
    public class Usuario
    {
        [Key]
        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }

        [Column("DNI")]
        public string? Dni { get; set; }

        [Column("NOMBRES")]
        public string Nombres { get; set; } = string.Empty;

        [Column("APELLIDOS")]
        public string Apellidos { get; set; } = string.Empty;

        [Column("CORREO")]
        public string Email { get; set; } = string.Empty;

        [Column("USERNAME")]
        public string Username { get; set; } = string.Empty;

        [Column("PASSWORD_HASH")]
        public string Password { get; set; } = string.Empty;

        [Column("ID_ROL")]
        public int IdRol { get; set; }

        [ForeignKey("IdRol")]
        public Rol? Rol { get; set; }

        [Column("ID_AREA")]
        public int? IdArea { get; set; }

        [Column("CARGO")]
        public string? Cargo { get; set; }

        [Column("ESTADO")]
        public int? Activo { get; set; }

        // Propiedad calculada para facilitar la visualización
        [NotMapped]
        public string NombreCompleto => $"{Nombres} {Apellidos}".Trim();
    }
}