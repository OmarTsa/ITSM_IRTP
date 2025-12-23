using System.Text.Json.Serialization;
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
        public string? Nombres { get; set; }

        [Column("APELLIDOS")]
        public string? Apellidos { get; set; }

        // Propiedad calculada para solucionar los errores CS1061
        [NotMapped]
        public string NombreCompleto => $"{Nombres} {Apellidos}".Trim();

        [Column("CORREO")]
        public string? Email { get; set; }

        [Column("USERNAME")]
        public string? Username { get; set; }

        [Column("PASSWORD_HASH")]
        [JsonIgnore]
        public string? Password { get; set; }

        [Column("ID_ROL")]
        public int IdRol { get; set; }

        [Column("ID_AREA")]
        public int? IdArea { get; set; }

        [Column("CARGO")]
        public string? Cargo { get; set; }

        [Column("ESTADO")]
        public int Activo { get; set; } = 1;

        [Column("FECHA_BAJA")]
        public DateTime? FechaBaja { get; set; }

        [ForeignKey("IdRol")]
        public virtual Rol? Rol { get; set; }
    }
}