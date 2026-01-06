namespace ITSM.Entidades.DTOs
{
    public class SesionDto
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        
        // ⭐ NUEVAS PROPIEDADES PARA TRAZABILIDAD
        public string NombreCompleto { get; set; } = string.Empty;
        public int? IdArea { get; set; }
        public string? NombreArea { get; set; }
    }
}
