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

        // Propiedad calculada para que no rompa el resto del código
        [NotMapped]
        public string NombreCompleto => $"{Nombres} {Apellidos}".Trim();

        [Column("CORREO")]
        public string Email { get; set; } = string.Empty;

        [Column("USERNAME")]
        public string Username { get; set; } = string.Empty;

        // Mapeamos el campo de la BD a nuestra propiedad Password
        [Column("PASSWORD_HASH")]
        public string Password { get; set; } = string.Empty;

        [Column("ID_ROL")]
        public int IdRol { get; set; }

        [ForeignKey("IdRol")]
        public Rol? Rol { get; set; }

        [Column("ESTADO")]
        public int? Activo { get; set; } // 1 = Activo
    }
}