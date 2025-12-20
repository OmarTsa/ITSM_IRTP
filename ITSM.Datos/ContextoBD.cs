using Microsoft.EntityFrameworkCore;
using ITSM.Entidades;

namespace ITSM.Datos
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
            // Esquema definido en tu DDL de Oracle
            modelBuilder.HasDefaultSchema("OTITO");

            modelBuilder.Entity<Usuario>().ToTable("SEG_USUARIOS");
            modelBuilder.Entity<Activo>().ToTable("ACT_INVENTARIO");
            modelBuilder.Entity<Ticket>().ToTable("HD_TICKETS");

            // MAPEO CRÍTICO: HD_CATEGORIAS contiene los 5 registros del DDL
            modelBuilder.Entity<Categoria>().ToTable("HD_CATEGORIAS");

            modelBuilder.Entity<Prioridad>().ToTable("PRIORIDADES");
            modelBuilder.Entity<EstadoTicket>().ToTable("ESTADOS_TICKET");

            // Data inicial para Prioridades (Seed Data)
            modelBuilder.Entity<Prioridad>().HasData(
                new Prioridad { IdPrioridad = 1, Nombre = "Alta (Crítico)", HorasSLA = 4 },
                new Prioridad { IdPrioridad = 2, Nombre = "Media (Normal)", HorasSLA = 24 },
                new Prioridad { IdPrioridad = 3, Nombre = "Baja (Planificado)", HorasSLA = 72 }
            );
        }
    }
}