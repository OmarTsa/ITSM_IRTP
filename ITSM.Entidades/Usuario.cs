using System;
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
        [Required(ErrorMessage = "El DNI es obligatorio")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener 8 dígitos")]
        public string Dni { get; set; }

        [Column("NOMBRES")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombres { get; set; }

        [Column("APELLIDOS")]
        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(100)]
        public string Apellidos { get; set; }

        [Column("CORREO")]
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        [StringLength(100)]
        public string Correo { get; set; }

        [Column("USERNAME")]
        [Required(ErrorMessage = "El usuario es obligatorio")]
        [StringLength(50)]
        public string Username { get; set; }

        [Column("PASSWORD_HASH")]
        [Required]
        public string PasswordHash { get; set; }

        [Column("CARGO")]
        [StringLength(100)]
        public string? Cargo { get; set; }

        [Column("ID_ROL")]
        public int IdRol { get; set; }

        [ForeignKey("IdRol")]
        public virtual Rol? Rol { get; set; }

        [Column("ID_AREA")]
        public int IdArea { get; set; }
        // Agrega la propiedad virtual Area si decides crear la entidad Area

        [Column("ESTADO")]
        public int Estado { get; set; } = 1; // 1=Activo, 0=Inactivo

        [Column("FECHA_BAJA")]
        public DateTime? FechaBaja { get; set; }

        // Propiedad auxiliar (no mapeada a BD) para mostrar nombre completo en las vistas
        [NotMapped]
        public string NombreCompleto => $"{Nombres} {Apellidos}";
    }
}