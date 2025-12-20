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

        [Column("USERNAME")]
        public string Username { get; set; } = string.Empty;

        [Column("PASSWORD_HASH")]
        public string PasswordHash { get; set; } = string.Empty;

        [Column("NOMBRES")]
        public string Nombres { get; set; } = string.Empty;

        [Column("APELLIDOS")]
        public string Apellidos { get; set; } = string.Empty;

        [Column("CORREO")]
        public string Correo { get; set; } = string.Empty;

        [Column("ID_ROL")]
        public int IdRol { get; set; }

        [Column("ESTADO")]
        public int Estado { get; set; }

        // --- Propiedades Auxiliares (Puente con tu Frontend) ---
        [NotMapped]
        public string NombreUsuario { get => Username; set => Username = value; }

        [NotMapped]
        public string Clave { get => PasswordHash; set => PasswordHash = value; }

        [NotMapped]
        public string NombreCompleto => $"{Nombres} {Apellidos}";

        [NotMapped]
        public string Rol => (IdRol == 1) ? "ADMIN" : "USUARIO";
    }
}