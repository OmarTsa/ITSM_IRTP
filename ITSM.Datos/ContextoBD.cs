using Microsoft.EntityFrameworkCore;
using ITSM.Entidades;

namespace ITSM.Datos
{
    public class ContextoBD : DbContext
    {
        public ContextoBD(DbContextOptions<ContextoBD> options) : base(options) { }

        // Tablas principales
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Activo> Activos { get; set; }

        // FALTABAN ESTAS DOS:
        public DbSet<Prioridad> Prioridades { get; set; }
        public DbSet<EstadoTicket> EstadosTicket { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuraciones adicionales si fueran necesarias
            base.OnModelCreating(modelBuilder);
        }
    }
}