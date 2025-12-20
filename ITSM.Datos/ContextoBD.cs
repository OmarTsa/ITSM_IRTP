using Microsoft.EntityFrameworkCore;
using ITSM.Entidades;

namespace ITSM.Datos
{
    public class ContextoBD : DbContext
    {
        public ContextoBD(DbContextOptions<ContextoBD> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Prioridad> Prioridades { get; set; }
        public DbSet<EstadoTicket> Estados { get; set; }
        public DbSet<Activo> Activos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones de nombres de tabla para Oracle
            modelBuilder.Entity<Usuario>().ToTable("SEG_USUARIOS");
            modelBuilder.Entity<Ticket>().ToTable("HD_TICKETS");
            modelBuilder.Entity<EstadoTicket>().ToTable("HD_ESTADOS");
            modelBuilder.Entity<Activo>().ToTable("ACT_INVENTARIO");
        }
    }
}