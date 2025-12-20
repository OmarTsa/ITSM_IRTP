using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades;

[Table("SEG_USUARIOS")]
public class Usuario
{
    [Key]
    [Column("ID_USUARIO")]
    public int IdUsuario { get; set; }

    [Column("ID_ROL")]
    public int IdRol { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [Column("NOMBRE_COMPLETO")]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
    [Column("NOMBRE_USUARIO")]
    public string NombreUsuario { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [Column("CORREO")]
    public string Correo { get; set; } = string.Empty;

    [Required]
    [Column("CLAVE")]
    public string Clave { get; set; } = string.Empty;

    [Column("ROL")]
    public string Rol { get; set; } = "Usuario";

    // Propiedades para ciberseguridad y autenticación
    [NotMapped] public string Username { get => NombreUsuario; set => NombreUsuario = value; }
    [NotMapped] public string PasswordHash { get => Clave; set => Clave = value; }
    [NotMapped] public int Estado { get; set; } = 1;
}