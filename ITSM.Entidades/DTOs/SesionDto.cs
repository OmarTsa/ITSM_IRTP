namespace ITSM.Entidades.DTOs
{
    public class SesionDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}