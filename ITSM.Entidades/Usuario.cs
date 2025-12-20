using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entidades
{
    public class Usuario
    {
        // --- 1. PROPIEDADES REALES (Mapeadas a la Base de Datos) ---
        public int IdUsuario { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public int IdRol { get; set; }
        public int Estado { get; set; }

        // --- 2. PROPIEDADES AUXILIARES (Para compatibilidad con tu código actual) ---
        // Estas propiedades sirven de "puente" para que no se rompa tu Login.razor ni tus controladores.

        [NotMapped]
        public string NombreUsuario
        {
            get => Username;
            set => Username = value;
        }

        [NotMapped]
        public string Clave
        {
            get => PasswordHash;
            set => PasswordHash = value;
        }

        [NotMapped]
        public string NombreCompleto => $"{Nombres} {Apellidos}";

        [NotMapped]
        public string Rol => (IdRol == 1) ? "ADMIN" : "USUARIO"; // Lógica simple para mostrar el rol
    }
}