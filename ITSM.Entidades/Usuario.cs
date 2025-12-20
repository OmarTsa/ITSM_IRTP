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

        [Required]
        [Column("NOMBRE_COMPLETO")]
        [StringLength(200)]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required]
        [Column("CORREO")]
        [StringLength(100)]
        public string Correo { get; set; } = string.Empty;

        [Required]
        [Column("CLAVE")]
        [StringLength(100)]
        public string Clave { get; set; } = string.Empty;

        [Column("ROL")]
        [StringLength(50)]
        public string Rol { get; set; } = "Usuario";

        [Column("ACTIVO")]
        public int Activo { get; set; } = 1;

        // Propiedades de ayuda (No se crean en la BD)
        [NotMapped]
        public string Username { get => Correo; set => Correo = value; }

        [NotMapped]
        public string Estado => Activo == 1 ? "Activo" : "Inactivo";
    }
}