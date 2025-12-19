using Microsoft.EntityFrameworkCore;
using ITSM.Entidades;

namespace ITSM.Datos // <--- ESTA LÍNEA ES CRÍTICA
{
    public class ContextoBD : DbContext
    {
        public ContextoBD(DbContextOptions<ContextoBD> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Activo> Activos { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Prioridad> Prioridades { get; set; }
        public DbSet<EstadoTicket> EstadosTicket { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("OTITO"); // Tu usuario Oracle

            // Mapeo de tablas
            modelBuilder.Entity<Usuario>().ToTable("USUARIOS");
            modelBuilder.Entity<Activo>().ToTable("ACTIVOS");
            modelBuilder.Entity<Ticket>().ToTable("TICKETS");
            modelBuilder.Entity<Categoria>().ToTable("CATEGORIAS");
            modelBuilder.Entity<Prioridad>().ToTable("PRIORIDADES");
            modelBuilder.Entity<EstadoTicket>().ToTable("ESTADOS_TICKET");

            // Datos semilla (opcional, para evitar errores si las tablas están vacías)
            modelBuilder.Entity<Prioridad>().HasData(
                new Prioridad { IdPrioridad = 1, Nombre = "Alta", HorasSLA = 4 },
                new Prioridad { IdPrioridad = 2, Nombre = "Media", HorasSLA = 24 }
            );
        }
    }
}