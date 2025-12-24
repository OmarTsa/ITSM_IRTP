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
        public string Dni { get; set; } = string.Empty;

        [Column("NOMBRES")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombres { get; set; } = string.Empty;

        [Column("APELLIDOS")]
        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(100)]
        public string Apellidos { get; set; } = string.Empty;

        [Column("CORREO")]
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        [StringLength(100)]
        public string Correo { get; set; } = string.Empty; // OJO: Se llama Correo, no Email

        [Column("USERNAME")]
        [Required(ErrorMessage = "El usuario es obligatorio")]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Column("PASSWORD_HASH")]
        public string PasswordHash { get; set; } = string.Empty;

        // --- CAMPOS AUXILIARES (NO EN BASE DE DATOS) ---
        // Necesarios para que el Formulario funcione sin errores
        [NotMapped]
        public string? Password { get; set; }

        [Column("CARGO")]
        [StringLength(100)]
        public string? Cargo { get; set; }

        [Column("ID_ROL")]
        public int IdRol { get; set; }

        [ForeignKey("IdRol")]
        public virtual Rol? Rol { get; set; }

        [Column("ID_AREA")]
        public int IdArea { get; set; }

        [ForeignKey("IdArea")]
        public virtual Area? Area { get; set; }

        [Column("ESTADO")]
        public int Estado { get; set; } = 1; // 1=Activo, 0=Inactivo

        [Column("FECHA_BAJA")]
        public DateTime? FechaBaja { get; set; }

        [Column("FECHA_CREACION")] // Necesario para evitar errores en ContextoBD
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [NotMapped]
        public string NombreCompleto => $"{Nombres} {Apellidos}";
    }
}