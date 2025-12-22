using Microsoft.EntityFrameworkCore;
using ITSM.Entidades;

namespace ITSM.Datos
{
    public class ContextoBD : DbContext
    {
        public ContextoBD(DbContextOptions<ContextoBD> options) : base(options) { }

        // TABLAS
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Activo> Activos { get; set; }

        // CATÁLOGOS (Asegúrate que estén aquí)
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Prioridad> Prioridades { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Rol> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configuraciones adicionales si las tienes...
        }
    }
}