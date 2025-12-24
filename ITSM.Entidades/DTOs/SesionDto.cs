namespace ITSM.Entidades.DTOs
{
    public class SesionDto
    {
        public string Nombre { get; set; }     // Ej: Juan Perez
        public string Username { get; set; }   // Ej: otito
        public string Rol { get; set; }        // Ej: Administrador
        public string Token { get; set; }      // El JWT
    }
}